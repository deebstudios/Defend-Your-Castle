using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //An action for an enemy to perform
    //All enemies start with an action!
    public abstract class Action
    {
        public enum ActionType
        {
            None, Idle, Moving, Attacking
        };

        //Enemy reference
        protected Enemy enemy;

        //An animation corresponding to the action
        protected Animation Anim;

        //The type of action this is (Attacking, Moving, etc.)
        protected ActionType Type;

        //Whether the action is complete or not
        protected bool Complete;

        private Action()
        {
            Type = ActionType.None;
        }

        public Action(Enemy enem, Animation anim, ActionType type) : this()
        {
            enemy = enem;
            Anim = anim;
            Anim.Restart();
            Type = type;
        }

        public Animation GetAnim
        {
            get { return Anim; }
        }

        public ActionType GetActionType
        {
            get { return Type; }
        }

        public bool IsComplete
        {
            get { return Complete; }
        }

        public virtual void Update(Level level)
        {
            Anim.Update();
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            float depth = enemy.GetDrawDepth;

            if (enemy.IsInvincible == true && enemy.GetInvincibleSheet != null) 
            {
                Anim.Draw(spriteBatch, enemy.GetInvincibleSheet, enemy.GetTruePosition, enemy.GetDirection, enemy.GetInvincibilityColor(true), 0f, depth + .0001f);
            }

            Anim.Draw(spriteBatch, spritesheet, enemy.GetTruePosition, enemy.GetDirection, enemy.GetColor, 0f, depth);
        }
    }
}
