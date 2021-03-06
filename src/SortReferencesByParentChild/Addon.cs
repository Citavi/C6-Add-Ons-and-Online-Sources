﻿using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.SortReferencesByParentChildAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {
        // EventHandlers

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is MainForm mainForm)
            {

                if (mainForm.Project.References.Comparer is ReferenceComparerByParentChild)
                {
                    mainForm.Project.AddComparerStatus();
                }
                else
                {
                    mainForm.Project.RemoveComparerStatus();
                }

                mainForm.FormClosing -= MainForm_FormClosing;
            }
        }

        // Methods

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            if (!mainForm.IsPreviewFullScreenForm)
            {
                var button = mainForm
                             .GetReferenceEditorNavigationCommandbarManager()
                             .GetCommandbar(MainFormReferenceEditorNavigationCommandbarId.Toolbar)
                             .GetCommandbarMenu(MainFormReferenceEditorNavigationCommandbarMenuId.Sort)
                             .AddCommandbarButton(ButtonKey, Properties.Resources.ParentChild);
                button.HasSeparator = true;

                if (mainForm.Project.RestoreComparer())
                {
                    mainForm.Project.References.Comparer = ReferenceComparerByParentChild.Default;
                    mainForm.Project.References.AutoSort = true;
                    Settings.Remove(SettingsKey);
                }

                mainForm.FormClosing += MainForm_FormClosing;
            }
        }

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(ButtonKey, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    e.Handled = true;
                    mainForm.Project.References.Comparer = ReferenceComparerByParentChild.Default;
                    mainForm.Project.References.AutoSort = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(e.Form, exception.Message, e.Form.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetReferenceEditorNavigationCommandbarManager()
                                 .GetCommandbar(MainFormReferenceEditorNavigationCommandbarId.Toolbar)
                                 .GetCommandbarMenu(MainFormReferenceEditorNavigationCommandbarMenuId.Sort)
                                 .GetCommandbarButton(ButtonKey);

            if (button != null)
            {
                button.Text = Properties.Resources.ParentChild;
            }
        }

        public override void OnApplicationIdle(MainForm mainForm)
        {
            if (mainForm.Project.References.Comparer is ReferenceComparerByParentChild)
            {
                if (mainForm.ProjectShell.NavigationGridItemFilter == null)
                {
                    mainForm.ProjectShell.NavigationGridItemFilter = new ItemFilter(mainForm);
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }

                //we MUST display child items indented if not yet the case
                if (!(mainForm.ProjectShell.NavigationGridItemFilter is ItemFilter))
                {
                    mainForm.ProjectShell.NavigationGridItemFilter = new ItemFilter();
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }
            }
            else
            {
                if (mainForm.ProjectShell.NavigationGridItemFilter != null)
                {
                    mainForm.ProjectShell.NavigationGridItemFilter = null;
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }

                //we MUST NOT display child items indented
                if (mainForm.ProjectShell.NavigationGridItemFilter is ItemFilter)
                {
                    mainForm.ProjectShell.NavigationGridItemFilter = null;
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }

            }
        }
    }
}