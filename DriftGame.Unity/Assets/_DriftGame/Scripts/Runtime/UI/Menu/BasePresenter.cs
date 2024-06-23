using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DriftGame.UI
{
    public abstract class BasePresenter
    {
        public VisualElement RootElement;
        public event Action OnToggle;

        protected BasePresenter(VisualElement rootElement)
        {
            RootElement = rootElement;
        }

        public void ToggleTo(BasePresenter newPage, params BasePresenter[] otherPages)
        {
            RootElement.AddToClassList("page__left");
            newPage.RootElement.RemoveFromClassList("page__left");
            newPage.OnToggle?.Invoke();

            foreach (BasePresenter page in otherPages)
            {
                page.RootElement.RemoveFromClassList("page__left");
            }
        }

        public void ToggleTo(params BasePresenter[] otherPages)
        {
            Debug.Log("Toggled");
            RootElement.RemoveFromClassList("page__left");
            foreach (BasePresenter page in otherPages)
            {
                if (page != this)
                {
                    page.RootElement.AddToClassList("page__left");
                }
            }
        }

        protected abstract void InitPage();
    }
}
