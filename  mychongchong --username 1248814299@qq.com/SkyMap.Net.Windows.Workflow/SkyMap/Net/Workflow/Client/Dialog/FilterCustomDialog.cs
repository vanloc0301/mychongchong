namespace SkyMap.Net.Workflow.Client.Dialog
{
    using DevExpress.Data.Filtering;
    using DevExpress.Data.Helpers;
    using DevExpress.LookAndFeel;
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Filter.Parser;
    using DevExpress.XtraGrid.FilterEditor;
    using DevExpress.XtraGrid.Localization;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class FilterCustomDialog : XtraForm
    {
        private CriteriaOperator _OldFilter;
        private SimpleButton btnCancel;
        private SimpleButton btnOK;
        internal GridColumn column;
        private Container components;
        internal BaseEdit e1;
        internal BaseEdit e2;
        protected GroupControl grbMain;
        private Label label1;
        internal object[,] operators;
        protected ComboBoxEdit piFirst;
        protected ComboBoxEdit piSecond;
        protected PanelControl pnlMain;
        protected CheckEdit rbAnd;
        protected CheckEdit rbOr;

        public FilterCustomDialog(GridColumn col) : this(col, true)
        {
        }

        public FilterCustomDialog(GridColumn col, bool setFilterValue)
        {
            this._OldFilter = null;
            this.operators = new object[,] { { ExpressionOperators.Equals, ExpressionOperators.NotEquals, ExpressionOperators.Greater, ExpressionOperators.GreaterOrEqual, ExpressionOperators.Less, ExpressionOperators.LessOrEqual, ExpressionOperators.Blanks, ExpressionOperators.NonBlanks, ExpressionOperators.Like, ExpressionOperators.NotLike }, { GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogConditionEQU), GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogConditionNEQ), GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogConditionGT), GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogConditionGTE), GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogConditionLT), GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogConditionLTE), GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogConditionBlanks), GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogConditionNonBlanks), GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogConditionLike), GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogConditionNotLike) }, { "=", "<>", ">", ">=", "<", "<=", "Is Null", "Is Not Null", "Like", "Not Like" }, { true, true, false, false, false, false, true, true, false, false } };
            this.components = null;
            this.column = col;
            this.InitializeComponent();
            base.ClientSize = new Size(0x182, 0xad);
            this.btnOK.Enabled = false;
            base.Icon = null;
            this._OldFilter = CriteriaOperator.Clone(this.column.FilterInfo.FilterCriteria);
            this.AddPickItems(this.piFirst);
            if (this.GetColumnType().Equals(typeof(string)))
            {
                this.piFirst.SelectedIndex = this.piFirst.Properties.Items.Count - 2;
            }
            else
            {
                this.piFirst.SelectedIndex = 1;
            }
            this.rbAnd.Checked = true;
            this.AddPickItems(this.piSecond);
            this.piSecond.SelectedItem = "";
            this.CreateEditor(ref this.e1, this.piFirst, 1);
            this.CreateEditor(ref this.e2, this.piSecond, 2);
            this.grbMain.Text = this.GetSpaceString(this.column.GetTextCaption());
            if (setFilterValue)
            {
                this.SetFilter();
            }
            this.SetLookAndFeel(base.LookAndFeel.ActiveLookAndFeel);
            this.Text = GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogFormCaption);
            this.label1.Text = GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogCaption);
            this.rbOr.Properties.Caption = GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogRadioOr);
            this.rbAnd.Properties.Caption = GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogRadioAnd);
            this.btnOK.Text = GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogOkButton);
            this.btnCancel.Text = GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogCancelButton);
        }

        private void AddPickItems(ComboBoxEdit pi)
        {
            pi.Properties.Items.Clear();
            pi.Properties.Items.Add("");
            int upperBound = this.operators.GetUpperBound(1);
            bool flag = true;
            if (!this.GetColumnType().Equals(typeof(string)))
            {
                upperBound -= 2;
            }
            for (int i = 0; i <= upperBound; i++)
            {
                if (this.GetColumnType().Equals(typeof(bool)) || this.GetColumnType().Equals(typeof(object)))
                {
                    flag = Convert.ToBoolean(this.operators[3, i]);
                }
                if (this.GetColumnType().Equals(typeof(byte[])))
                {
                    flag = Convert.ToBoolean(this.operators[3, i]) && (i > 1);
                }
                if (flag)
                {
                    object item = this.operators[1, i].ToString();
                    pi.Properties.Items.Add(item);
                }
            }
        }

        private bool AssignFirstFilter(CriteriaOperator op)
        {
            return this.SetValue(op, this.piFirst, this.e1);
        }

        private bool AssignSecondFilter(CriteriaOperator op)
        {
            return this.SetValue(op, this.piSecond, this.e2);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CriteriaOperator filtersCriterion = this.GetFiltersCriterion();
            this.CreateFilter(filtersCriterion);
        }

        internal void ChangeValues()
        {
            if (this.piFirst.SelectedItem != null)
            {
                bool flag = (this.piFirst.SelectedItem.Equals("") && this.piSecond.SelectedItem.Equals("")) && !object.ReferenceEquals(this._OldFilter, null);
                string str = flag ? GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogClearFilter) : GridLocalizer.Active.GetLocalizedString(GridStringId.CustomFilterDialogOkButton);
                this.btnOK.Enabled = this.IsFilterExist(this.piFirst, this.e1) || flag;
                if (this.btnOK.Text != str)
                {
                    this.btnOK.Text = str;
                }
            }
        }

        internal void comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBoxEdit edit = sender as ComboBoxEdit;
            if ((edit.SelectedIndex < 0) || edit.SelectedItem.Equals(""))
            {
                edit.BackColor = this.GetDisabledColor();
            }
            else
            {
                edit.BackColor = this.GetEnabledColor();
            }
            this.ChangeValues();
        }

        private void CreateEditor(ref BaseEdit e, ComboBoxEdit pi, int index)
        {
            e = this.CreateEditorEx(e);
            e.Properties.BorderStyle = BorderStyles.Default;
            e.Properties.AllowFocused = true;
            e.Properties.AutoHeight = false;
            e.Properties.ReadOnly = false;
            e.Enabled = true;
            e.Location = new Point((pi.Left + pi.Width) + 20, pi.Top);
            e.Size = new Size((this.grbMain.Width - e.Left) - pi.Left, this.piFirst.Height);
            e.TabIndex = (index == 1) ? 3 : 0x1a;
            e.EditValue = null;
            e.EditValueChanged += new EventHandler(this.EditValueChanged);
            pi.SelectedValueChanged += new EventHandler(this.comboBox_SelectedValueChanged);
            e.Validated += new EventHandler(this.OnValidated);
            pi.Tag = index;
            e.KeyDown += new KeyEventHandler(this.EditorKeyDown);
            this.SetColor(e);
            this.comboBox_SelectedValueChanged(pi, null);
            this.grbMain.Controls.Add(e);
            if (e is ButtonEdit)
            {
                ((ButtonEdit) e).Properties.TextEditStyle = TextEditStyles.Standard;
            }
        }

        protected virtual BaseEdit CreateEditorEx(BaseEdit e)
        {
            BaseEdit edit;
            if (this.column.RealColumnEdit is RepositoryItemProgressBar)
            {
                edit = new SpinEdit();
            }
            else if (this.column.RealColumnEdit is RepositoryItemMemoEdit)
            {
                edit = new MemoExEdit();
                ((MemoExEdit) edit).Properties.ShowIcon = false;
            }
            else if (this.IsLookUpEditors() && this.IsDisplayTextFilter())
            {
                RepositoryItem item = GridCriteriaHelper.CreateDisplayTextEditor(this.CurrentColumn);
                edit = item.CreateEditor();
                edit.Properties.Assign(item);
            }
            else
            {
                edit = this.column.RealColumnEdit.CreateEditor();
                edit.Properties.ResetEvents();
                edit.Properties.Assign(this.column.RealColumnEdit);
            }
            if (edit is ComboBoxEdit)
            {
                ((ComboBoxEdit) edit).Properties.UseCtrlScroll = false;
            }
            if (edit is ImageComboBoxEdit)
            {
                ((ImageComboBoxEdit) edit).Properties.UseCtrlScroll = false;
            }
            TextEdit edit2 = edit as TextEdit;
            if (edit2 != null)
            {
                edit2.Properties.AllowNullInput = DefaultBoolean.True;
            }
            return edit;
        }

        private void CreateFilter(CriteriaOperator filterCriteria)
        {
            try
            {
                if (!object.ReferenceEquals(filterCriteria, null))
                {
                    ColumnFilterInfo info = new ColumnFilterInfo(filterCriteria);
                    this.column.FilterInfo = info;
                }
                else
                {
                    this.column.FilterInfo = new ColumnFilterInfo();
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(base.LookAndFeel.ActiveLookAndFeel, base.FindForm(), exception.Message, GridLocalizer.Active.GetLocalizedString(GridStringId.WindowErrorCaption));
                this.CreateFilter(this._OldFilter);
            }
        }

        internal string DescriptionByExOperator(ExpressionOperators o)
        {
            int num = 0;
            num = 0;
            while (num <= this.operators.GetUpperBound(1))
            {
                if (this.operators[0, num].Equals(o))
                {
                    break;
                }
                num++;
            }
            return this.operators[1, num].ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EditorKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) && e.Control)
            {
                ((BaseEdit) sender).EditValue = null;
            }
        }

        private void EditValueChanged(object sender, EventArgs e)
        {
            if (!this.IsImmediatePopup(sender))
            {
                this.SetColor(sender);
                this.ChangeValues();
            }
        }

        private ExpressionOperators ExOperatorByDescription(object o)
        {
            string str = o.ToString();
            int num = 0;
            num = 0;
            while (num <= this.operators.GetUpperBound(1))
            {
                if (this.operators[1, num].ToString() == str)
                {
                    break;
                }
                num++;
            }
            return (ExpressionOperators) this.operators[0, num];
        }

        private bool ExtractOpa(CriteriaOperator op, out ExpressionOperators eopa, out CriteriaOperator operand)
        {
            UnaryOperator objA = op as UnaryOperator;
            bool flag = !object.ReferenceEquals(objA, null) && (objA.OperatorType == UnaryOperatorType.Not);
            if (flag)
            {
                op = objA.Operand;
            }
            UnaryOperator operator2 = op as UnaryOperator;
            if (!object.ReferenceEquals(operator2, null))
            {
                operand = null;
                if (operator2.OperatorType == UnaryOperatorType.IsNull)
                {
                    eopa = flag ? ExpressionOperators.NonBlanks : ExpressionOperators.Blanks;
                    return true;
                }
            }
            BinaryOperator operator3 = op as BinaryOperator;
            if (!object.ReferenceEquals(operator3, null))
            {
                operand = operator3.RightOperand;
                if (flag)
                {
                    switch (operator3.OperatorType)
                    {
                        case BinaryOperatorType.Equal:
                            eopa = ExpressionOperators.NotEquals;
                            return true;

                        case BinaryOperatorType.NotEqual:
                            eopa = ExpressionOperators.Equals;
                            return true;

                        case BinaryOperatorType.Greater:
                            eopa = ExpressionOperators.LessOrEqual;
                            return true;

                        case BinaryOperatorType.Less:
                            eopa = ExpressionOperators.GreaterOrEqual;
                            return true;

                        case BinaryOperatorType.LessOrEqual:
                            eopa = ExpressionOperators.Greater;
                            return true;

                        case BinaryOperatorType.GreaterOrEqual:
                            eopa = ExpressionOperators.Less;
                            return true;

                        case BinaryOperatorType.Like:
                            eopa = ExpressionOperators.NotLike;
                            return true;
                    }
                }
                else
                {
                    switch (operator3.OperatorType)
                    {
                        case BinaryOperatorType.Equal:
                            eopa = ExpressionOperators.Equals;
                            return true;

                        case BinaryOperatorType.NotEqual:
                            eopa = ExpressionOperators.NotEquals;
                            return true;

                        case BinaryOperatorType.Greater:
                            eopa = ExpressionOperators.Greater;
                            return true;

                        case BinaryOperatorType.Less:
                            eopa = ExpressionOperators.Less;
                            return true;

                        case BinaryOperatorType.LessOrEqual:
                            eopa = ExpressionOperators.LessOrEqual;
                            return true;

                        case BinaryOperatorType.GreaterOrEqual:
                            eopa = ExpressionOperators.GreaterOrEqual;
                            return true;

                        case BinaryOperatorType.Like:
                            eopa = ExpressionOperators.Like;
                            return true;
                    }
                }
            }
            operand = null;
            eopa = ExpressionOperators.Blanks;
            return false;
        }

        [Obsolete("Use GetFiltersCriterion instead")]
        protected virtual string FilterConditions()
        {
            CriteriaOperator filtersCriterion = this.GetFiltersCriterion();
            if (object.ReferenceEquals(filtersCriterion, null))
            {
                return string.Empty;
            }
            return filtersCriterion.ToString();
        }

        protected System.Type GetColumnType()
        {
            if (this.IsDisplayTextFilter())
            {
                return typeof(string);
            }
            return this.column.ColumnType;
        }

        private Color GetDisabledColor()
        {
            if (base.LookAndFeel.ActiveLookAndFeel.ActiveStyle != ActiveLookAndFeelStyle.Skin)
            {
                return ColorUtils.FlatTabBackColor;
            }
            return LookAndFeelHelper.GetSystemColor(base.LookAndFeel.ActiveLookAndFeel, SystemColors.Control);
        }

        private Color GetEnabledColor()
        {
            return LookAndFeelHelper.GetSystemColor(base.LookAndFeel.ActiveLookAndFeel, SystemColors.Window);
        }

        protected CriteriaOperator GetFilterCriterion(ComboBoxEdit pi, BaseEdit e)
        {
            return this.GetFilterCriterion(pi, e, false, string.Empty);
        }

        protected CriteriaOperator GetFilterCriterion(ComboBoxEdit pi, BaseEdit e, bool IsField, string FieldName)
        {
            if (this.IsFilterExist(pi, e))
            {
                OperandProperty opLeft = new OperandProperty(this.column.FieldName);
                int num = this.OperationNumber(pi.SelectedItem);
                if (num < 0)
                {
                    return null;
                }
                switch (((ExpressionOperators) this.operators[0, num]))
                {
                    case ExpressionOperators.Equals:
                        return (CriteriaOperator) (opLeft == this.GetRightOperand(e, IsField, FieldName));

                    case ExpressionOperators.NotEquals:
                        return (CriteriaOperator) (opLeft != this.GetRightOperand(e, IsField, FieldName));

                    case ExpressionOperators.Greater:
                        return (CriteriaOperator) (opLeft > this.GetRightOperand(e, IsField, FieldName));

                    case ExpressionOperators.GreaterOrEqual:
                        return (CriteriaOperator) (opLeft >= this.GetRightOperand(e, IsField, FieldName));

                    case ExpressionOperators.Less:
                        return (CriteriaOperator) (opLeft < this.GetRightOperand(e, IsField, FieldName));

                    case ExpressionOperators.LessOrEqual:
                        return (CriteriaOperator) (opLeft <= this.GetRightOperand(e, IsField, FieldName));

                    case ExpressionOperators.Blanks:
                        return opLeft.IsNull();

                    case ExpressionOperators.NonBlanks:
                        return opLeft.IsNotNull();

                    case ExpressionOperators.Like:
                        return new BinaryOperator(opLeft, this.PatchLikeOperand(this.GetRightOperand(e, IsField, FieldName)), BinaryOperatorType.Like);
                    //暂时未处理
                    //case ExpressionOperators.NotLike:
                    //    return CriteriaOperator.op_LogicalNot(new BinaryOperator(opLeft, this.PatchLikeOperand(this.GetRightOperand(e, IsField, FieldName)), BinaryOperatorType.Like));
                }
            }
            return null;
        }

        protected virtual CriteriaOperator GetFiltersCriterion()
        {
            return GroupOperator.Combine(this.GroupOperatorType, this.GetFilterCriterion(this.piFirst, this.e1), this.GetFilterCriterion(this.piSecond, this.e2));
        }

        private CriteriaOperator GetRightOperand(BaseEdit e, bool IsField, string FieldName)
        {
            if (IsField)
            {
                return new OperandProperty(FieldName);
            }
            return new OperandValue(FilterHelper.CorrectFilterValueType(this.GetColumnType(), e.EditValue));
        }

        private string GetSpaceString(string ret)
        {
            ret = ret.Replace("\n", " ");
            ret = ret.Replace("\r", "");
            ret = ret.Replace("\t", " ");
            return ret;
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.grbMain = new GroupControl();
            this.rbOr = new CheckEdit();
            this.rbAnd = new CheckEdit();
            this.piSecond = new ComboBoxEdit();
            this.piFirst = new ComboBoxEdit();
            this.btnOK = new SimpleButton();
            this.btnCancel = new SimpleButton();
            this.pnlMain = new PanelControl();
            this.grbMain.BeginInit();
            this.grbMain.SuspendLayout();
            this.rbOr.Properties.BeginInit();
            this.rbAnd.Properties.BeginInit();
            this.piSecond.Properties.BeginInit();
            this.piFirst.Properties.BeginInit();
            this.pnlMain.BeginInit();
            this.pnlMain.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x61, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Show rows where:";
            this.grbMain.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.grbMain.Controls.AddRange(new Control[] { this.rbOr, this.rbAnd, this.piSecond, this.piFirst });
            this.grbMain.Location = new Point(8, 0x18);
            this.grbMain.Name = "grbMain";
            this.grbMain.Size = new Size(0x174, 0x6c);
            this.grbMain.TabIndex = 1;
            this.rbOr.Location = new Point(0x5e, 0x34);
            this.rbOr.Name = "rbOr";
            this.rbOr.Properties.Caption = "&Or";
            this.rbOr.Properties.CheckStyle = CheckStyles.Radio;
            this.rbOr.Properties.RadioGroupIndex = 0;
            this.rbOr.Size = new Size(0x38, 20);
            this.rbOr.TabIndex = 14;
            this.rbAnd.Location = new Point(0x26, 0x34);
            this.rbAnd.Name = "rbAnd";
            this.rbAnd.Properties.Caption = "&And";
            this.rbAnd.Properties.CheckStyle = CheckStyles.Radio;
            this.rbAnd.Properties.RadioGroupIndex = 0;
            this.rbAnd.Size = new Size(0x38, 20);
            this.rbAnd.TabIndex = 13;
            this.piSecond.EditValue = "";
            this.piSecond.Location = new Point(12, 0x4c);
            this.piSecond.Name = "piSecond";
            this.piSecond.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.piSecond.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.piSecond.Size = new Size(0xa4, 0x15);
            this.piSecond.TabIndex = 0x19;
            this.piFirst.EditValue = "";
            this.piFirst.Location = new Point(12, 0x1c);
            this.piFirst.Name = "piFirst";
            this.piFirst.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.piFirst.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.piFirst.Size = new Size(0xa4, 0x15);
            this.piFirst.TabIndex = 2;
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0xbc, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x5c, 0x19);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x120, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x5c, 0x19);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.pnlMain.BorderStyle = BorderStyles.NoBorder;
            this.pnlMain.Controls.AddRange(new Control[] { this.btnCancel, this.btnOK, this.grbMain, this.label1 });
            this.pnlMain.Dock = DockStyle.Fill;
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new Size(0x184, 0xaf);
            this.pnlMain.TabIndex = 0;
            base.AcceptButton = this.btnOK;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x184, 0xaf);
            base.Controls.AddRange(new Control[] { this.pnlMain });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FilterCustomDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Custom AutoFilter";
            this.grbMain.EndInit();
            this.grbMain.ResumeLayout(false);
            this.rbOr.Properties.EndInit();
            this.rbAnd.Properties.EndInit();
            this.piSecond.Properties.EndInit();
            this.piFirst.Properties.EndInit();
            this.pnlMain.EndInit();
            this.pnlMain.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected bool IsDisplayTextFilter()
        {
            return (this.column.FilterMode == ColumnFilterMode.DisplayText);
        }

        protected virtual bool IsFilterExist(ComboBoxEdit pi, BaseEdit e)
        {
            bool flag = true;
            if ((((pi.SelectedItem == null) || pi.SelectedItem.Equals("")) || (this.column.FieldName == "")) || ((!pi.SelectedItem.Equals(this.operators[1, 6]) && !pi.SelectedItem.Equals(this.operators[1, 7])) && (e.EditValue == null)))
            {
                flag = false;
            }
            return flag;
        }

        private bool IsImmediatePopup(object sender)
        {
            return ((sender is LookUpEditBase) && ((LookUpEditBase) sender).Properties.ImmediatePopup);
        }

        protected bool IsLookUpEditors()
        {
            return ((((this.column.ColumnEdit is RepositoryItemImageComboBox) || (this.column.ColumnEdit is RepositoryItemLookUpEditBase)) || (this.column.ColumnEdit is RepositoryItemPopupContainerEdit)) || (this.column.ColumnEdit is RepositoryItemRadioGroup));
        }

        private void OnValidated(object sender, EventArgs e)
        {
            this.SetColor(sender);
        }

        private int OperationNumber(object exo)
        {
            for (int i = 0; i <= this.operators.GetUpperBound(1); i++)
            {
                if (this.operators[1, i].Equals(exo))
                {
                    return i;
                }
            }
            return -1;
        }

        private CriteriaOperator PatchLikeOperand(CriteriaOperator op)
        {
            OperandValue objA = op as OperandValue;
            if (!object.ReferenceEquals(objA, null) && (objA.Value != null))
            {
                string str = objA.Value.ToString().Replace('*', '%').Replace('?', '_');
                if (!str.StartsWith("%"))
                {
                    str = "%" + str;
                }
                if (!str.EndsWith("%"))
                {
                    str = str + "%";
                }
                return new OperandValue(str);
            }
            return op;
        }

        private void SetColor(object sender)
        {
            BaseEdit edit = sender as BaseEdit;
            if (edit.EditValue == null)
            {
                edit.Properties.Appearance.BackColor = this.GetDisabledColor();
            }
            else
            {
                edit.Properties.Appearance.BackColor = this.GetEnabledColor();
            }
        }

        protected virtual bool SetEditorValue(ComboBoxEdit pi, BaseEdit e, CriteriaOperator operand)
        {
            OperandValue objA = operand as OperandValue;
            if (!object.ReferenceEquals(objA, null))
            {
                e.EditValue = objA.Value;
                return true;
            }
            return false;
        }

        internal void SetFilter()
        {
            CriteriaOperator objA = this._OldFilter;
            if (!object.ReferenceEquals(objA, null))
            {
                GroupOperator operator2 = objA as GroupOperator;
                if (object.ReferenceEquals(operator2, null) || (operator2.Operands.Count < 2))
                {
                    this.AssignFirstFilter(objA);
                }
                else
                {
                    this.rbAnd.Checked = operator2.OperatorType == DevExpress.Data.Filtering.GroupOperatorType.And;
                    this.rbOr.Checked = operator2.OperatorType == DevExpress.Data.Filtering.GroupOperatorType.Or;
                    if (this.AssignFirstFilter(operator2.Operands[0]))
                    {
                        this.AssignSecondFilter(operator2.Operands[1]);
                    }
                }
            }
        }

        internal void SetLookAndFeel(UserLookAndFeel lookAndFeel)
        {
            if ((this.column.View.GridControl == null) || !this.column.View.GridControl.FormsUseDefaultLookAndFeel)
            {
                base.LookAndFeel.Assign(lookAndFeel);
                this.pnlMain.LookAndFeel.Assign(lookAndFeel);
                this.grbMain.LookAndFeel.Assign(lookAndFeel);
                foreach (Control control in this.pnlMain.Controls)
                {
                    this.SetLookAndFeelControl(control, lookAndFeel);
                }
                foreach (Control control in this.grbMain.Controls)
                {
                    this.SetLookAndFeelControl(control, lookAndFeel);
                }
            }
        }

        private void SetLookAndFeelControl(Control c, UserLookAndFeel lookAndFeel)
        {
            BaseControl control = c as BaseControl;
            if (control != null)
            {
                control.LookAndFeel.Assign(lookAndFeel);
            }
        }

        private bool SetValue(CriteriaOperator opa, ComboBoxEdit pi, BaseEdit e)
        {
            ExpressionOperators operators;
            CriteriaOperator @operator;
            if (!this.ExtractOpa(opa, out operators, out @operator))
            {
                return false;
            }
            pi.SelectedItem = this.DescriptionByExOperator(operators);
            return this.SetEditorValue(pi, e, @operator);
        }

        protected GridColumn CurrentColumn
        {
            get
            {
                return this.column;
            }
        }

        internal DevExpress.Data.Filtering.GroupOperatorType GroupOperatorType
        {
            get
            {
                return (this.rbAnd.Checked ? DevExpress.Data.Filtering.GroupOperatorType.And : DevExpress.Data.Filtering.GroupOperatorType.Or);
            }
        }
    }
}

