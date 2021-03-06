﻿using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Resources;
using SwissAcademic.WordProcessing;
using System.Collections.Generic;
using System.Linq;

namespace SwissAcademic.Addons.SortReferencesByParentChildAddon
{
    public class ItemFilter : IReferenceNavigationGridDisplayItemFilter
    {
        // Fields

        readonly MainForm _mainForm;

        // Constructors

        public ItemFilter(MainForm mainForm) => _mainForm = mainForm;

        public ItemFilter()
        {
            if (Program.ProjectShells.Count == 0)
            {
                _mainForm = null;
            }
            else
            {
                _mainForm = Program.ProjectShells[0].PrimaryMainForm;
            }
        }

        // Methods

        List<Reference> GetAvailableParents()
        {
            return
                _mainForm?
                .GetFilteredReferences()
                .FindAll(reference => reference?.ReferenceType?.AllowedChildren?.Count > 0) ?? new List<Reference>();
        }

        public bool Filters(ReferenceNavigationGridDisplayItemColumn column) => true;

        public string GetValue(Reference reference, ReferenceNavigationGridDisplayItemColumn column, out bool handled)
        {
            if (reference.ParentReference == null)
            {
                handled = false;
                return string.Empty;
            }


            var parentAvailable = GetAvailableParents().Any(parent => reference.ParentReference == parent);
            if (!parentAvailable)
            {
                handled = false;
                return string.Empty;
            }

            handled = true;
            var spacer = new string('\u00A0', 11);
            switch (column)
            {
                #region AuthorsOrEditorsOrOrganizations

                case ReferenceNavigationGridDisplayItemColumn.AuthorsOrEditorsOrOrganizations:
                    {
                        string result;

                        if (reference.HasCoreField(ReferenceTypeCoreFieldId.Authors) && reference.Authors.Count != 0)
                        {
                            if (reference.Authors.Count <= 10) result = reference.Authors.ToString();
                            else result = reference.Authors.Take(11).ToString("; ");
                        }

                        else if (reference.HasCoreField(ReferenceTypeCoreFieldId.Editors) && reference.Editors.Count != 0)
                        {
                            if (reference.Editors.Count == 1) result = string.Concat(reference.Editors, " (", Strings.Ed, ")");
                            else if (reference.Editors.Count > 1 && reference.Editors.Count <= 10) result = string.Concat(reference.Editors, " (", Strings.Eds, ")");
                            else result = string.Concat(reference.Editors.Take(11).ToString("; "), " (", Strings.Eds, ")");
                        }

                        else if (reference.HasCoreField(ReferenceTypeCoreFieldId.Organizations) && reference.Organizations.Count != 0)
                        {
                            if (reference.Organizations.Count <= 10) result = reference.Organizations.ToString();
                            else result = reference.Organizations.Take(11).ToString("; ");
                        }

                        else
                        {
                            result = "–";
                        }

                        return spacer + result;
                    }

                #endregion

                #region BibTeXKey

                case ReferenceNavigationGridDisplayItemColumn.BibTeXKey:
                    {
                        string result;

                        if (string.IsNullOrEmpty(reference.BibTeXKey)) result = "–";
                        else result = reference.BibTeXKey;

                        return spacer + result;
                    }


                #endregion

                #region CitationKey

                case ReferenceNavigationGridDisplayItemColumn.CitationKey:
                    {
                        string result;

                        if (string.IsNullOrEmpty(reference.CitationKey)) result = "–";
                        else result = reference.CitationKey;

                        return spacer + result;
                    }

                #endregion

                #region Title

                case ReferenceNavigationGridDisplayItemColumn.Title:
                    {
                        string result;

                        var title = string.IsNullOrEmpty(reference.TitleTagged) ? string.Empty : reference.TitleTagged.CssStyleTagsToHtmlStyleTags();
                        var subtitle = string.IsNullOrEmpty(reference.SubtitleTagged) ? string.Empty : reference.SubtitleTagged.CssStyleTagsToHtmlStyleTags();

                        if (string.IsNullOrEmpty(title))
                        {
                            if (string.IsNullOrEmpty(subtitle)) result = "–";
                            else result = subtitle;
                        }

                        else if (string.IsNullOrEmpty(subtitle))
                        {
                            result = title;
                        }

                        else
                        {
                            switch (reference.Title[reference.Title.Length - 1])
                            {
                                case '.':
                                case ',':
                                case '?':
                                case '!':
                                case ':':
                                case ';':
                                    result = string.Concat(title, " ", subtitle);
                                    break;

                                default:
                                    result = string.Concat(title, ". ", subtitle);
                                    break;
                            }
                        }

                        return "&nbsp;&nbsp;&nbsp;&nbsp;" + result;
                    }

                #endregion

                #region YearAndReferenceType

                case ReferenceNavigationGridDisplayItemColumn.YearAndReferenceType:
                    {
                        string result;

                        if (string.IsNullOrEmpty(reference.YearResolved)) result = reference.ReferenceType.NameLocalized;
                        else result = reference.YearResolved + " – " + reference.ReferenceType.NameLocalized;

                        return spacer + result;
                    }

                #endregion

                #region default

                default:
                    {
                        handled = false;
                        return string.Empty;
                    }

                    #endregion
            }
        }

    }
}
