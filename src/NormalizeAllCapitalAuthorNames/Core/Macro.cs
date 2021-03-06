﻿using SwissAcademic.Addons.NormalizeAllCapitalAuthorNamesAddon.Properties;
using SwissAcademic.Citavi.Shell;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.NormalizeAllCapitalAuthorNamesAddon
{
    internal static class Macro
    {
        public static void Run(PersonList personList)
        {
            var prefixSuffixFirstCapitalLetter = false;
            var normalizeCapitalLastname = true;
            var counter = 0;
            var authors = personList.Project.Persons.ToList();

            foreach (var author in authors)
            {
                var originalAuthorFullName = author.FullName.ToString();
                var originalAuthorLastName = author.LastName.ToString();

                if ((!string.IsNullOrEmpty(originalAuthorFullName) && originalAuthorFullName.Equals(originalAuthorFullName.ToUpper(), StringComparison.Ordinal)) ||
                    (!string.IsNullOrEmpty(originalAuthorLastName) && originalAuthorLastName.Equals(originalAuthorLastName.ToUpper(), StringComparison.Ordinal) && normalizeCapitalLastname))
                {
                    counter++;

                    var authorFirstName = author.FirstName.ToString();
                    var authorMiddleName = author.MiddleName.ToString();
                    var authorLastName = author.LastName.ToString();

                    if (!string.IsNullOrEmpty(authorFirstName)) author.FirstName = authorFirstName.ToLower().ToInitialUpper();
                    if (!string.IsNullOrEmpty(authorMiddleName)) author.MiddleName = authorMiddleName.ToLower().ToInitialUpper();
                    if (!string.IsNullOrEmpty(authorLastName)) author.LastName = authorLastName.ToLower().ToInitialUpper();

                    var authorPrefix = author.Prefix.ToString();
                    var authorSuffix = author.Suffix.ToString();

                    if (prefixSuffixFirstCapitalLetter == true)
                    {
                        if (!string.IsNullOrEmpty(authorPrefix)) author.Prefix = authorPrefix.ToLower().ToInitialUpper();
                        if (!string.IsNullOrEmpty(authorSuffix)) author.Suffix = authorSuffix.ToLower().ToInitialUpper();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(authorPrefix)) author.Prefix = authorPrefix.ToLower();
                        if (!string.IsNullOrEmpty(authorSuffix)) author.Suffix = authorSuffix.ToLower();
                    }

                }
            }

            if (authors.Any())
            {
                MessageBox.Show(personList, Resources.ResultMessage.FormatString(counter), personList.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}