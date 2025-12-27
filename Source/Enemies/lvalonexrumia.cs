using System;
using System.Collections.Generic;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Neutral.Black;
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.GunName;
using lvalonexrumia.Patches;
using lvalonexrumia.StatusEffects;
using lvalonmeme.StatusEffects;


namespace lvalonexrumia.Enemies
{
	[EntityLogic(typeof(lvalonexrumiaEnemyUnitDef))]
	public sealed class lvalonexrumia : EnemyUnit
	{

		// Move:
		//0 - "Blood Sign"
		//1 - "Crimson Moonlight Sonata" gun2
		//2 - "Ten-Shaded Slash" gun3
		//3 - "Shadow Cut" gun4
		//4 - "Nether Shade"
		//5 - "Shadow Magia Form"
		//6 - "EX"
		//7 - "Termination" gun1
		//Internal list of the boss moves
		private enum MoveType
		{
			EX,
			CMS,
			Ten,
			Shadow,
			Nether,
			Magia //cms -> nether -> shadow -> MAGIA -> ex -> ten -> magia -> SHADOW -> cms
		}

		//Internal parameters use to track the last move used by the boss.
		private MoveType Last { get; set; }

		private MoveType Next { get; set; }

		//Get the moves names
		public string move1 //ex
		{
			get
			{
				return base.GetSpellCardName(new int?(6), 7);
				//return base.GetMove(0);
			}
		}

		public string move2 //cms
		{
			get
			{
				return base.GetSpellCardName(new int?(0), 1);
			}
		}
		bool halved = false;
		bool halftriggered = false;
		bool firstturn = true;
		bool go = false;
		GameDifficulty diff;

		protected override void OnEnterBattle(BattleController battle)
		{
			go = false;
			halftriggered = false;
			firstturn = true;
			diff = base.GameRun.Difficulty;
			halved = this.Hp < toolbox.hpfrompercent(this, 50, true) ? true : false;
			this.Last = MoveType.Magia;
			this.Next = MoveType.CMS;
			//AudioManager.PlayInLayer1("AyaBossZUN2Bgm");
			ReactBattleEvent(DamageReceived, OnDamageReceived);
			ReactBattleEvent(CustomGameEventManager.PostChangeLifeEvent, OnLifeChanged);
			ReactBattleEvent(DamageDealt, OnDamageDealt);
		}
		public override void OnSpawn(EnemyUnit spawner)
		{
			this.React(new ApplyStatusEffectAction<MirrorImage>(this, null, null, null, null, 0f, true));
		}

		private IEnumerable<BattleAction> OnDamageDealt(DamageEventArgs args)
		{
			if (halved && args.Source == this)
			{
				if (args.ActionSource is StatusEffect se && se.Owner == this)
				{
					yield break;
				}
				yield return new ChangeLifeAction(toolbox.hpfrompercent(this, 1, true), this);
			}
		}

		private IEnumerable<BattleAction> OnLifeChanged(ChangeLifeEventArgs args)
		{
			if (halved)
			{
				yield break;
			}
			halved = this.Hp < toolbox.hpfrompercent(this, 50, true) ? true : false;
			if (!halftriggered && halved)
			{
				if (diff == GameDifficulty.Hard || diff == GameDifficulty.Lunatic)
				{
					yield return new ApplyStatusEffectAction<Firepower>(this, 1, 0, 0, 0, 0.2f);
				}
				yield return new ApplyStatusEffectAction<sebossheal>(this, 1, 0, 0, 0, 0.2f);
				halftriggered = true;
			}
			yield break;
		}

		private IEnumerable<BattleAction> OnDamageReceived(DamageEventArgs args)
		{
			halved = this.Hp < toolbox.hpfrompercent(this, 50, true) ? true : false;
			if (!halftriggered && halved)
			{
				if (diff == GameDifficulty.Hard || diff == GameDifficulty.Lunatic)
				{
					yield return new ApplyStatusEffectAction<Firepower>(this, 1, 0, 0, 0, 0.2f);
				}
				yield return new ApplyStatusEffectAction<sebossheal>(this, 1, 0, 0, 0, 0.2f);
				halftriggered = true;
			}
			yield break;
		}

		//Action for the turn.
		protected override IEnumerable<IEnemyMove> GetTurnMoves()
		{
			switch (this.Next)
			{
				case MoveType.EX:
					{
						yield return new SimpleEnemyMove(Intention.NegativeEffect(), this.EXAction1());
						yield return new SimpleEnemyMove(Intention.SpellCard(move1, Damage1, 3, true), this.EXAction2());
						if (halved)
						{
							yield return new SimpleEnemyMove(Intention.Heal());
						}
						this.Last = MoveType.EX;
						yield break;
					}
				case MoveType.Ten:
					{
						if (halved)
						{
							yield return new SimpleEnemyMove(Intention.NegativeEffect(), this.TenAction1());
						}
						yield return new SimpleEnemyMove(Intention.Attack(Damage3, 10, false).WithMoveName(base.GetMove(2)), this.TenAction2());
						yield return new SimpleEnemyMove(Intention.AddCard(), this.AddShadow());
						if (halved)
						{
							yield return new SimpleEnemyMove(Intention.Heal());
						}
						yield return new SimpleEnemyMove(Intention.CountDown(2));
						this.Last = MoveType.Ten;
						yield break;
					}
				case MoveType.Shadow:
					{
						yield return new SimpleEnemyMove(Intention.Attack(Damage4, 4, false).WithMoveName(base.GetMove(3)), this.ShadowAction());
						yield return new SimpleEnemyMove(Intention.AddCard(), this.AddShadow());
						if (halved)
						{
							yield return new SimpleEnemyMove(Intention.Heal());
						}
						if (go == true)
						{
							yield return new SimpleEnemyMove(Intention.CountDown(1));
						}
						else
						{
							yield return new SimpleEnemyMove(Intention.CountDown(2));
						}
						this.Last = MoveType.Shadow;
						yield break;
					}
				case MoveType.Nether:
					{
						yield return new SimpleEnemyMove(Intention.Defend().WithMoveName(base.GetMove(4)), this.NetherAction());
						yield return new SimpleEnemyMove(Intention.AddCard(), this.AddShadow());
						this.Last = MoveType.Nether;
						yield break;
					}
				case MoveType.Magia:
					{
						if (!go)
						{
							yield return new SimpleEnemyMove(Intention.NegativeEffect(), this.MagiaAction1());
						}
						yield return new SimpleEnemyMove(Intention.Graze().WithMoveName(base.GetMove(5)), this.MagiaAction2());
						if (go == true)
						{
							yield return new SimpleEnemyMove(Intention.CountDown(1));
						}
						else
						{
							yield return new SimpleEnemyMove(Intention.CountDown(2));
						}
						this.Last = MoveType.Magia;
						yield break;
					}

				case MoveType.CMS:
					{
						yield return new SimpleEnemyMove(Intention.NegativeEffect(), this.CMSAction1());
						//yield return new SimpleEnemyMove(Intention.SpellCard(move2, Damage2, 2, true), this.CMSAction2());
						yield return new SimpleEnemyMove(Intention.SpellCard(move2, Damage2, 2, true), this.CMSAction2());
						if (halved)
						{
							yield return new SimpleEnemyMove(Intention.Heal());
						}
						if (!firstturn)
						{
							yield return new SimpleEnemyMove(Intention.PositiveEffect(), this.CMSAction3());
						}
						this.Last = MoveType.CMS;
						yield break;
					}
			}
			yield break;
		}

		//Perform a custom action
		private IEnumerable<BattleAction> EXAction1()
		{
			yield return new ApplyStatusEffectAction<sebloodmark>(Battle.Player, halved ? 2 : 1, 0, 0, 0, 0.2f);
		}
		private IEnumerable<BattleAction> EXAction2()
		{
			yield return PerformAction.Spell(this, "exulta");
			foreach (BattleAction action in this.AttackActions(move1, Gun1, Damage1, 3, true))
			{
				yield return action;
			}
			// for (int i = 0; i < 3; i++)
			// {
			// 	yield return new DamageAction(this, new[] { Battle.Player }, DamageInfo.Attack(Damage1, true), Gun1, GunType.Single);
			// }
		}
		private IEnumerable<BattleAction> TenAction1()
		{
			yield return new ApplyStatusEffectAction<Vulnerable>(Battle.Player, 0, 2, 0, 0, 0.2f);
		}
		private IEnumerable<BattleAction> TenAction2()
		{
			foreach (BattleAction action in this.AttackActions(base.GetMove(2), Gun3, Damage3, 10, false, Gun3))
			{
				yield return action;
			}
		}
		private IEnumerable<BattleAction> AddShadow()
		{
			IEnumerable<Card> cards = Library.CreateCards<Shadow>(halved && diff == GameDifficulty.Lunatic ? 2 : 1, false);
			foreach (Card card in cards)
			{
				card.IsPurified = true;
			}
			yield return new AddCardsToDrawZoneAction(cards, DrawZoneTarget.Random);
		}
		private IEnumerable<BattleAction> ShadowAction()
		{
			foreach (BattleAction action in this.AttackActions(base.GetMove(3), Gun4, Damage4, 4, false, Gun3))
			{
				yield return action;
			}
		}
		private IEnumerable<BattleAction> NetherAction()
		{
			yield return new CastBlockShieldAction(this, base.Defend, halved ? base.Defend : 0);
		}
		private IEnumerable<BattleAction> MagiaAction1()
		{
			yield return new ApplyStatusEffectAction<Vulnerable>(Battle.Player, 0, 2, 0, 0, 0.2f);
		}
		private IEnumerable<BattleAction> MagiaAction2()
		{
			yield return new ApplyStatusEffectAction<Graze>(this, (diff == GameDifficulty.Lunatic || diff == GameDifficulty.Hard ? 2 : 1) + (halved ? 1 : 0), 0, 0, 0, 0.2f);
		}
		private IEnumerable<BattleAction> CMSAction1()
		{
			yield return new ApplyStatusEffectAction<sebleed>(Battle.Player, Count1 + (halved ? 2 : 0), 0, 0, 0, 0.2f);
		}
		private IEnumerable<BattleAction> CMSAction2()
		{
			yield return PerformAction.Spell(this, "exultb");
			foreach (BattleAction action in this.AttackActions(move2, Gun2, Damage2, 2, true, Gun2))
			{
				yield return action;
			}
			firstturn = false;
		}
		private IEnumerable<BattleAction> CMSAction3()
		{
			yield return new ApplyStatusEffectAction<Firepower>(this, 1, 0, 0, 0, 0.2f);
		}

		//Update choose the next attack.
		protected override void UpdateMoveCounters()
		{
			switch (this.Last)
			{
				case MoveType.EX:
					{
						this.Next = MoveType.Ten;
						break;
					}
				case MoveType.CMS:
					{
						this.Next = MoveType.Nether;
						break;
					}
				case MoveType.Nether:
					{
						this.Next = MoveType.Shadow;
						break;
					}
				case MoveType.Shadow:
					{
						if (go == true)
						{
							this.Next = MoveType.CMS;
							go = false;
						}
						else
						{
							this.Next = MoveType.Magia;
							go = true;
						}
						break;
					}
				case MoveType.Ten:
					{
						this.Next = MoveType.Magia;
						break;
					}
				case MoveType.Magia:
					{
						if (go == true)
						{
							this.Next = MoveType.EX;
							go = false;
						}
						else
						{
							this.Next = MoveType.Shadow;
							go = true;
						}
						break;
					}
			}
		}
	}
}