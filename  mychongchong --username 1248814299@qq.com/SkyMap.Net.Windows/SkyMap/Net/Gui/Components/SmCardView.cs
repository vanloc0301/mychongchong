namespace SkyMap.Net.Gui.Components
{
    using DevExpress.Utils;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Card;
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class SmCardView : CardView
    {
        public SmCardView()
        {
            base.MaximumCardColumns = 1;
            base.Appearance.FieldValue.TextOptions.WordWrap = WordWrap.Wrap;
            base.Appearance.FieldCaption.BackColor = Color.GhostWhite;
            base.Appearance.FieldCaption.BackColor = Color.GhostWhite;
            base.Appearance.FieldCaption.ForeColor = Color.BlueViolet;
            base.Appearance.FieldCaption.Font = new Font("宋体", 9f, FontStyle.Bold);
            base.Appearance.FieldCaption.TextOptions.VAlignment = VertAlignment.Center;
            base.VertScrollVisibility = ScrollVisibility.Never;
            base.OptionsBehavior.FieldAutoHeight = true;
            base.OptionsBehavior.AutoHorzWidth = true;
            base.OptionsBehavior.AutoFocusNewCard = true;
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

        public int GetRowHandle(object o)
        {
            for (int i = this.RowCount - 1; i > -1; i--)
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
    }
}

