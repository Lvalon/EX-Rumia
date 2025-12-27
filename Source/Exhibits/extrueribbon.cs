using System;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Patches;
using lvalonexrumia.StatusEffects;

namespace lvalonexrumia.Exhibits
{
	public sealed class extrueribbonDef : lvalonexrumiaExhibitTemplate
	{
		public override ExhibitConfig MakeConfig()
		{
			ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();
			exhibitConfig.Order = 1;
			exhibitConfig.Value1 = 2;
			exhibitConfig.Value2 = 10;
			exhibitConfig.Owner = null;
			exhibitConfig.Revealable = true;
			exhibitConfig.LosableType = ExhibitLosableType.CantLose;
			exhibitConfig.Rarity = Rarity.Mythic;
			exhibitConfig.Appearance = AppearanceType.Nowhere;
			exhibitConfig.Mana = new ManaGroup() { Philosophy = 1 };
			exhibitConfig.BaseManaColor = ManaColor.Philosophy;
			exhibitConfig.RelativeEffects = new List<string>() { nameof(sebloodmark), nameof(sedeepbleed) };

			return exhibitConfig;
		}
	}

	[EntityLogic(typeof(extrueribbonDef))]
	public sealed class extrueribbon : ShiningExhibit
	{
		protected override void OnEnterBattle()
		{
			ReactBattleEvent(Battle.BattleStarting, new EventSequencedReactor<GameEventArgs>(OnBattleStarting));
		}
		private IEnumerable<BattleAction> OnBattleStarting(GameEventArgs args)
		{
			NotifyActivating();
			foreach (Unit unit in Battle.AllAliveEnemies)
			{
				for (int i = 0; i < Value1; i++)
				{
					if (Battle.BattleShouldEnd)
					{
						yield break;
					}
					int loss = Math.Max((int)(1f * unit.MaxHp * Value2 / 100), Value2);
					yield return new ChangeLifeAction(-loss, unit);
				}
				if (Battle.BattleShouldEnd)
				{
					yield break;
				}
				yield return new ApplyStatusEffectAction<sebloodmark>(unit, Value1, 0, 0, 0, 0.2f);
				yield return new ApplyStatusEffectAction<sedeepbleed>(unit, Value1, 0, 0, 0, 0.2f);
			}
			yield break;
		}

	}
}