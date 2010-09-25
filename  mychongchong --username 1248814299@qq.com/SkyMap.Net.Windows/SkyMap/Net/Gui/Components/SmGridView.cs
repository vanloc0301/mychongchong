namespace SkyMap.Net.Gui.Components
{
    using DevExpress.Utils;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using System;
    using System.Collections.Generic;

    public class SmGridView : GridView
    {
        public SmGridView()
        {
            this.SetDefaultAppearance();
        }

        public SmGridView(SmGridControl smGridControl) : base(smGridControl)
        {
            this.SetDefaultAppearance();
        }

        public void DeleteSelectedOrFocusedRows()
        {
            int[] selectedOrFocusedRowHandles = this.GetSelectedOrFocusedRowHandles();
            if (selectedOrFocusedRowHandles != null)
            {
                for (int i = selectedOrFocusedRowHandles.Length - 1; i > -1; i--)
                {
                    this.DeleteRow(selectedOrFocusedRowHandles[i]);
                }
            }
        }

        public T GetRow<T>(int rowHandle)
        {
            return (T) base.GetRow(rowHandle);
        }

        public int GetRowHandle(object o)
        {
            for (int i = base.DataRowCount - 1; i > -1; i--)
            {
                if (this.GetRow(i).Equals(o))
                {
                    return i;
                }
            }
            return -1;
        }

        public int[] GetSelectedOrFocusedRowHandles()
        {
            int[] selectedRows = this.GetSelectedRows();
            if (((selectedRows == null) || (selectedRows.Length == 0)) && (base.FocusedRowHandle > -1))
            {
                selectedRows = new int[] { base.FocusedRowHandle };
            }
            return selectedRows;
        }

        public List<T> GetSelectedOrFocusedRows<T>()
        {
            List<T> list = new List<T>();
            int[] selectedRows = this.GetSelectedRows();
            if (selectedRows != null)
            {
                foreach (int num in selectedRows)
                {
                    list.Add((T) this.GetRow(num));
                }
            }
            if ((list.Count == 0) && (base.FocusedRowHandle >= 0))
            {
                list.Add((T) this.GetRow(base.FocusedRowHandle));
            }
            return list;
        }

        protected override void OnLostFocus()
        {
            base.OnLostFocus();
            base.PostEditor();
        }

        private void SetDefaultAppearance()
        {
            base.OptionsMenu.EnableColumnMenu = false;
            base.OptionsMenu.EnableFooterMenu = false;
            base.OptionsMenu.EnableGroupPanelMenu = false;
            base.OptionsView.EnableAppearanceEvenRow = base.OptionsView.EnableAppearanceOddRow = true;
            base.CustomDrawColumnHeader += new ColumnHeaderCustomDrawEventHandler(this.SmGridView_CustomDrawColumnHeader);
            base.ScrollStyle = ScrollStyleFlags.LiveHorzScroll | ScrollStyleFlags.LiveVertScroll;
            base.HorzScrollVisibility = ScrollVisibility.Always;
            base.VertScrollVisibility = ScrollVisibility.Always;
            this.PaintStyleName = "Skin";
        }

        private void SmGridView_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            e.Info.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            e.Painter.DrawObject(e.Info);
        }
    }
}

