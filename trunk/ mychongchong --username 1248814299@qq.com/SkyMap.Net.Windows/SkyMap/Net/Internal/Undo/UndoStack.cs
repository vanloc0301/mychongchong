namespace SkyMap.Net.Internal.Undo
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public class UndoStack
    {
        public bool AcceptChanges = true;
        private Stack redostack = new Stack();
        private Stack undostack = new Stack();

        public event EventHandler ActionRedone;

        public event EventHandler ActionUndone;

        public void ClearAll()
        {
            this.undostack.Clear();
            this.redostack.Clear();
        }

        public void ClearRedoStack()
        {
            this.redostack.Clear();
        }

        protected void OnActionRedone()
        {
            if (this.ActionRedone != null)
            {
                this.ActionRedone(null, null);
            }
        }

        protected void OnActionUndone()
        {
            if (this.ActionUndone != null)
            {
                this.ActionUndone(null, null);
            }
        }

        public void Push(IUndoableOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("UndoStack.Push(UndoableOperation operation) : operation can't be null");
            }
            if (this.AcceptChanges)
            {
                this.undostack.Push(operation);
                this.ClearRedoStack();
            }
        }

        public void Redo()
        {
            if (this.redostack.Count > 0)
            {
                IUndoableOperation operation = (IUndoableOperation) this.redostack.Pop();
                this.undostack.Push(operation);
                operation.Redo();
                this.OnActionRedone();
            }
        }

        public void Undo()
        {
            if (this.undostack.Count > 0)
            {
                IUndoableOperation operation = (IUndoableOperation) this.undostack.Pop();
                this.redostack.Push(operation);
                operation.Undo();
                this.OnActionUndone();
            }
        }

        public void UndoLast(int x)
        {
            this.undostack.Push(new UndoQueue(this, x));
        }

        internal Stack _UndoStack
        {
            get
            {
                return this.undostack;
            }
        }

        public bool CanRedo
        {
            get
            {
                return (this.redostack.Count > 0);
            }
        }

        public bool CanUndo
        {
            get
            {
                return (this.undostack.Count > 0);
            }
        }
    }
}

