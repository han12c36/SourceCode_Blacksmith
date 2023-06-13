namespace CustomAction
{
    using Enums;
    using System;
    using System.Linq;
    using UnityEngine;
    public interface IActionTable { }

    public abstract class ActionTable<T, E> : MonoBehaviour, IActionTable where E : Enum
    {
        public T me;
        protected Action<T, E>[] actions;
        protected Action<T, E> prevAction, curAction;
        protected int i_prevAction, i_curAction;
        [SerializeField]
        public E e_prevAction, e_curAction;

        private Array array => Enum.GetValues(typeof(E));

        protected virtual void Awake()
        {
            me = GetComponent<T>();
            actions = new Action<T, E>[array.Length];
            i_prevAction = i_curAction = -1;
            e_prevAction = e_curAction = array.Cast<E>().First();

            Initialize();
        }
        protected virtual void Start() { }
        protected virtual void Update() { if (curAction != null) curAction.ActionUpdate(); }
        protected virtual void FixedUpdate() { if (curAction != null) curAction.ActionFixedUdpate(); }
        protected virtual void LateUpdate() { if (curAction != null) curAction.ActionLateUpdate(); }

        protected abstract void Initialize();

        protected void Add<ActionType>(E actionIndex) where ActionType : Action<T, E>, new()
        {
            ActionType action = new ActionType();
            action.me = this.me;
            action.actionTable = this;
            actions[Convert.ToInt32(actionIndex)] = action;
        }
        protected void Remove(E actionIndex)
        {
            int index = Convert.ToInt32(actionIndex);
            if (actions[index] == null) return;

            actions[index].me = default(T); //제네릭에서의 null
            actions[index].actionTable = null;
            actions[index] = null;
        }
        public void SetCurAction(E actionIndex)
        {
            int index = Convert.ToInt32(actionIndex);

            if (actions[index] == null) return; // 왜 안됨?

            int nextAction = index;

            if (curAction != null)
            {
                curAction.ActionExit();
                prevAction = curAction;

                i_prevAction = i_curAction;
                e_prevAction = (E)array.GetValue(i_prevAction);
            }

            curAction = actions[nextAction];
            i_curAction = nextAction;
            e_curAction = (E)array.GetValue(i_curAction);

            curAction.ActionEnter();
        }

        #region 오브젝트 로그 
        public void PrintText(string text)
        {
            string objectName = gameObject.name;
            Debug.Log($"[{objectName}] {text}");
        }
        #endregion
    }
    public abstract class Action<T, E> where E : Enum
    {
        public T me;
        public ActionTable<T, E> actionTable;

        public abstract void ActionEnter();
        public virtual void ActionUpdate() { }
        public virtual void ActionFixedUdpate() { }
        public virtual void ActionLateUpdate() { }
        public abstract void ActionExit();
    }
}

