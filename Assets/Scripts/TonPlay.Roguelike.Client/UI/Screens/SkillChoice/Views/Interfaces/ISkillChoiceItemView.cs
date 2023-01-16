using System;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces
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
	}
}