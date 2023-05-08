using System;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views.Interfaces
{
	public interface ISkillChoiceItemView : ICollectionItemView
	{
		IObservable<Unit> OnButtonClick { get; }

		void SetTitleText(string text);

		void SetDescriptionText(string text);

		void SetIcon(Sprite icon);

		void SetCurrentLevel(int level);

		void SetNextLevel(int level);

		void SetMaxLevel(int level);

		void SetBackgroundColor(Color color);

		void SetTitleTextColor(Color color);

		void SetLevelIcon(Sprite icon);
	}
}