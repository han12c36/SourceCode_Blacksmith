using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace MVP
{
    public interface IPresenter<G_Model,G_View>
    {
        public IModel<G_Model> Model { get; set; }
        public IView<G_View> View { get; set; }
        public G_Model model { get; }
        public G_View view { get; }
    }
    
    public interface IView<GModel> { }

    public interface IModel<GView> { public abstract void ModelInitialized(); }

    public abstract class Presenter<G_Model,G_View> : IPresenter<G_Model, G_View>
    {
        private IModel<G_Model> _model;
        private IView<G_View> _view;
        private object _lock = new object();
        public Action action;

        public G_Model model => (G_Model)_model;
        public G_View view => (G_View)_view;

        public IModel<G_Model> Model { get => _model; set => _model = value; }
        public IView<G_View> View { get => _view; set => _view = value; }

        public void OnClickEvent() { action?.Invoke(); action = null; }

        public void AddOnClickEvent(Action eventAction) 
            { lock (_lock) { action = eventAction; OnClickEvent(); } }
    }

    public class UserDataPresenter : Presenter<UserData, InterfaceUserInfo>
    {
        //model 과 view를 받는 생성자
        public UserDataPresenter(IModel<UserData> model,IView<InterfaceUserInfo> view)
        {
            Model = model; 
            Model.ModelInitialized();
            View  = view;  
            ViewInitialized();
        }

        public void ViewInitialized()
        {
            view.gold_Text.text = V_AddGold();
            view.level_Text.text = V_LevelUp();
            view.name_Text.text = V_ChangeName();
        }

        //중재자로써 주고받을 기능 구현

        //view -> presenter -> model -> presenter -> view
        public string V_AddGold(int addAmount = 0) 
        { 
            //view.goldFeedback.PlayFeedbacks();
            return model.AddGold(addAmount); 
        }
        public string V_LevelUp(int addLevel = 0) { return model.LevelUp(addLevel); }
        public string V_ChangeName(string name = Constants.DefaultUserName) { return model.ChangeName(name); }

        //model -> presenter -> view
        public void M_AddGold(string amount) 
        {
            //view.goldFeedback.PlayFeedbacks();
            view.gold_Text.text = amount; 
        }
        public void M_LevelUp(string level) { view.level_Text.text = level; }
        public void M_ChangeName(string name) { view.name_Text.text = name; }
    }
}
