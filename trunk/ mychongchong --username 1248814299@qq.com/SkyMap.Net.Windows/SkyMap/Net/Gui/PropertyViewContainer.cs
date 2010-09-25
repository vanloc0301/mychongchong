namespace SkyMap.Net.Gui
{
    using System;
    using System.Collections;

    public sealed class PropertyViewContainer
    {
        private ICollection selectableObjects;
        private object selectedObject;
        private object[] selectedObjects;

        public void Clear()
        {
            this.SelectableObjects = null;
            this.SelectedObject = null;
        }

        public ICollection SelectableObjects
        {
            get
            {
                return this.selectableObjects;
            }
            set
            {
                this.selectableObjects = value;
                PropertyView.UpdateSelectableIfActive(this);
            }
        }

        public object SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
            set
            {
                this.selectedObject = value;
                this.selectedObjects = null;
                PropertyView.UpdateSelectedObjectIfActive(this);
            }
        }

        public object[] SelectedObjects
        {
            get
            {
                return this.selectedObjects;
            }
            set
            {
                this.selectedObject = null;
                this.selectedObjects = value;
                PropertyView.UpdateSelectedObjectIfActive(this);
            }
        }
    }
}

