namespace CustomAction
{
    using System;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// IWeapon과 동일하게 부모 클래스에서 파생 클래스에 접근하기 위한 인터페이스
    /// </summary>
    public interface IActionTable { }

    /// <summary>
    /// 행위테이블 정의 : 사용자 즉 본인을 알아야하며, 사용할 열거형에 대한 명시가 있어야한다.
    /// => 해당 클래스를 파생시 위 부분을 모두 일일이 명시하기에는 상속구조의 장점을 헤친다.
    /// State Pattern으로 각각의 행위를 관리하여 행위 전환에 cost를 최소화 한다.
    /// </summary>
    /// <typeparam name="T">사용자(Owner)</typeparam>
    /// <typeparam name="E">사용될 열거형(enum)</typeparam>
    
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

            actions[index].me = default(T);
            actions[index].actionTable = null;
            actions[index] = null;
        }
        public void SetCurAction(E actionIndex)
        {
            int index = Convert.ToInt32(actionIndex);

            if (actions[index] == null) return;

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

    }

    /// <summary>
    /// ActionTable을 각 행위마다 알고 있어야한다.
    /// -> 사용자와 열거형을 명시해야한다.
    /// </summary>
    /// <typeparam name="T">ActionTable의 사용자(owner)</typeparam>
    /// <typeparam name="E">사용할 열거형(enum)</typeparam>
    
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

