﻿using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSnippetsReflection.LanguageGenerators
{
    public static class CommonGenerator
    {
        /// <summary>
        /// Language agnostic function to generate Query section of code snippet 
        /// </summary>
        /// <param name="snippetModel">Model of the snippet</param>
        /// <param name="languageExpressions">Instance of <see cref="LanguageExpressions"/> that holds the expressions for the specific language</param>
        public static string GenerateQuerySection(SnippetModel snippetModel, LanguageExpressions languageExpressions)
        {
            var snippetBuilder = new StringBuilder();

            //Append any headers section
            foreach (var (key, value) in snippetModel.RequestHeaders)
            {
                //no need to generate source for the host header
                if (key.ToLower().Equals("host"))
                    continue;
                //append the header to the snippet
                snippetBuilder.Append(string.Format(languageExpressions.HeaderExpression, key, value.First()));
            }
            //Append any filter queries
            if (snippetModel.FilterFieldList.Any())
            {
                var filterResult = GetListAsStringForSnippet(snippetModel.FilterFieldList, languageExpressions.FilterExpressionDelimiter);
                //append the filter to the snippet
                snippetBuilder.Append(string.Format(languageExpressions.FilterExpression, filterResult));
            }

            //Append any search queries
            if (!string.IsNullOrEmpty(snippetModel.SearchExpression))
            {
                snippetBuilder.Append(string.Format(languageExpressions.SearchExpression, snippetModel.SearchExpression));
            }

            //Append any expand queries
            if (snippetModel.ExpandFieldList.Any())
            {
                var expandResult = GetListAsStringForSnippet(snippetModel.ExpandFieldList, languageExpressions.ExpandExpressionDelimiter);
                //append the expand result to the snippet
                snippetBuilder.Append(string.Format(languageExpressions.ExpandExpression, expandResult));
            }

            //Append any select queries
            if (snippetModel.SelectFieldList.Any())
            {
                var selectResult = GetListAsStringForSnippet(snippetModel.SelectFieldList, languageExpressions.SelectExpressionDelimiter);
                //append the select result to the snippet
                snippetBuilder.Append(string.Format(languageExpressions.SelectExpression, selectResult));
            }

            //Append any orderby queries
            if (snippetModel.OrderByFieldList.Any())
            {
                var orderByResult = GetListAsStringForSnippet(snippetModel.OrderByFieldList, languageExpressions.OrderByExpressionDelimiter);
                //append the orderby result to the snippet
                snippetBuilder.Append(string.Format(languageExpressions.OrderByExpression, orderByResult));
            }

            //Append any skip queries
            if (snippetModel.ODataUri.Skip.HasValue)
            {
                snippetBuilder.Append(string.Format(languageExpressions.SkipExpression, snippetModel.ODataUri.Skip));
            }

            //Append any skip token queries
            if (!string.IsNullOrEmpty(snippetModel.ODataUri.SkipToken))
            {
                snippetBuilder.Append(string.Format(languageExpressions.SkipTokenExpression, snippetModel.ODataUri.SkipToken));
            }

            //Append any top queries
            if (snippetModel.ODataUri.Top.HasValue)
            {
                snippetBuilder.Append(string.Format(languageExpressions.TopExpression, snippetModel.ODataUri.Top));
            }

            return snippetBuilder.ToString();
        }

        public static string GetClassNameFromIdentifier(ODataPathSegment oDataPathSegment, string identifier)
        {
            IEdmType definition = null;
            string returnValue = "";
            switch (oDataPathSegment)
            {
                case NavigationPropertySegment navigationPropertySegment:
                    definition = navigationPropertySegment.NavigationProperty.Type.Definition;
                    returnValue = GetClassNameFromEdmType(definition, oDataPathSegment.Identifier, identifier);
                    break;
                case EntitySetSegment entitySetSegment:
                    definition = entitySetSegment.EdmType as IEdmCollectionType;
                    returnValue = GetClassNameFromEdmType(definition, oDataPathSegment.Identifier, identifier);
                    break;
                case OperationSegment operationSegment:
                    foreach (var parameters in operationSegment.Operations.First().Parameters)
                    {
                        if (parameters.Name.ToLower().Equals("bindingparameter") || parameters.Name.ToLower().Equals("bindparameter"))
                            continue;
                        definition = parameters.Type.Definition;
                        returnValue = GetClassNameFromEdmType(definition, parameters.Name, identifier);

                        if (!string.IsNullOrEmpty(returnValue))
                            break;
                    }
                    break;
                default:
                    break;
            }

            return returnValue;
        }

        private static string GetClassNameFromEdmType(IEdmType definition, string entityIdentifier, string searchParameter)
        {
            string returnString = "";
            var elementDefinition = definition;
            //if the type is a collection, use the type of the element
            if (definition is IEdmCollectionType type)
            {
                elementDefinition = type.ElementType.Definition;
            }

            //check is the entity identifier is what we want already
            if (entityIdentifier.Equals(searchParameter, StringComparison.OrdinalIgnoreCase))
            {
                return elementDefinition.ToString();
            }

            //Loop through the properties of the entity if is structured
            if (definition is IEdmStructuredType structuredType)
            {
                foreach (var property in structuredType.DeclaredProperties)
                {
                    if (property.Name.Equals(searchParameter))
                    {
                        if (property.Type.Definition is IEdmCollectionType innerCollection)
                        {
                            return innerCollection.ElementType.Definition.ToString();
                        }
                        else
                        {
                            return property.Type.Definition.ToString();
                        }
                    }
                }
            }

            return returnString.Split(".").Last();
        }


        /// <summary>
        /// Helper function to join string list into one string delimited with a desired character
        /// </summary>
        /// <param name="fieldList">List of strings that are to be concatenated to a string </param>
        /// <param name="delimiter">Delimiter to be used to join the string elements</param>
        private static string GetListAsStringForSnippet(IEnumerable<string> fieldList, string delimiter)
        {
            var result = new StringBuilder();
            foreach (var queryOption in fieldList)
            {
                result.Append(queryOption + delimiter);
            }
            if (!string.IsNullOrEmpty(delimiter))
            {
                result.Remove(result.Length - delimiter.Length, delimiter.Length);
            }

            return result.ToString();

        }
    }
}
