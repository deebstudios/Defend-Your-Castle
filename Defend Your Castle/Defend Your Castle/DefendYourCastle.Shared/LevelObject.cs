using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //The type of object the level object is; we only need a few types for comparison
    public enum ObjectType
    {
        None, Enemy, Projectile
    }

    public enum Direction
    {
        Right, Left
    };

    //A base class for objects that exist in each level; this includes the Player Castle and Enemies
    //Hitboxes, hurtboxes, and the like are added as children, but references to them are kept
    public abstract class LevelObject
    {
        //Gravitational constant for flying objects affected by gravity
        public const float Gravity = .1f;

        //Tells if the object is influenced by gravity or not (used mostly for projectiles)
        protected bool UsesGravity;

        //Whether the object is active or not
        protected bool Active;

        //The weapon type that hurts the object; anything equal to or above it can hurt it
        protected int WeaponWeakness;

        //Invincibility lasts for a designated amount of time that differs depending on the object that has it
        protected float InvincibilityLength;
        protected float PrevInvincibility;
        protected Fade InvincibilityFade;

        //Whether the object is dead or not and should be removed
        protected bool Dead;

        //The position of the object
        protected Vector2 Position;

        //The rotation of the object
        protected float Rotation;

        //The object type of the LevelObject
        protected ObjectType objectType;

        //The direction the object is facing; defaults to Right
        protected Direction DirectionFacing;

        //A potential hitbox the object can have
        protected Hitbox hitbox;

        //Hurtbox of the object
        protected Hurtbox hurtbox;

        //Spritesheet of the object; it can be accompanied by a sheet used when the object is invincible
        protected Texture2D ObjectSheet;
        protected Texture2D InvincibleSheet;

        //The animation of the object
        protected Animation Animation;

        //The parent of the level object
        protected LevelObject Parent;

        //Children of the level object
        protected List<LevelObject> Children;

        public LevelObject()
        {
            Position = Vector2.Zero;
            Rotation = 0f;
            objectType = ObjectType.None;
            DirectionFacing = Direction.Right;
            UsesGravity = false;

            //Set invincibililty to last 1 second by default
            InvincibilityLength = 1000f;
            PrevInvincibility = 0f;
            InvincibilityFade = Fade.Empty;

            ObjectSheet = null;
            InvincibleSheet = null;
            WeaponWeakness = (int)Player.WeaponTypes.Sword;

            Children = new List<LevelObject>();
        }

        public bool IsActive
        {
            get { return Active; }
        }

        //Used for certain things (Ex. Enemy gold effect showing but the enemy isn't dead)
        public virtual bool IsDying
        {
            get { return false; }
        }

        public bool IsDead
        {
            get { return Dead; }
        }

        //Checks if the object is invincible
        public bool IsInvincible
        {
            get { return (Game1.ActiveTime < PrevInvincibility); }
        }

        //The true position of the object - this is used in specific cases, such as the Flying Enemy's height
        public virtual Vector2 GetTruePosition
        {
            get { return Position; }
        }

        //Get the position of the object
        public Vector2 GetPosition
        {
            get { return Position; }
        }

        public int GetWeaponWeakness
        {
            get { return WeaponWeakness; }
        }

        public ObjectType GetObjectType
        {
            get { return objectType; }
        }

        //Get the direction the object is facing
        public Direction GetDirection
        {
            get { return DirectionFacing; }
        }

        //Gets the invincibility fade color or pure color
        public Color GetInvincibilityColor(bool fadecolor)
        {
            if (fadecolor == true)
                return InvincibilityFade.GetFadeColor;
            else return InvincibilityFade.GetColor;
        }

        //Gets the invincibility's current fade
        public float GetInvincibilityFade
        {
            get { return InvincibilityFade.GetCurFade; }
        }

        //Gets an object's invincible spritesheet
        public Texture2D GetInvincibleSheet
        {
            get { return InvincibleSheet; }
        }

        //Sets the hitbox of the object
        public void SetHitbox(int width, int height)
        {
            hitbox = new Hitbox(Position, width, height);
            AddChild(hitbox);
        }

        //Sets the hurtbox of the object
        public void SetHurtbox(int width, int height, Vector2 padamount)
        {
            hurtbox = new Hurtbox(GetTruePosition, width, height, padamount);
            AddChild(hurtbox);
        }

        //Get the proper depth to draw the object; objects lower in the Y position will be drawn above objects above them
        public float GetDrawDepth
        {
            get 
            {
                float depth = (Position.Y / 1000f);
                if (depth <= 0) depth = .001f;

                return depth;
            }
        }

        public Hurtbox GetHurtbox
        {
            get { return hurtbox; }
        }

        //Sets the parent of this object
        public void SetParent(LevelObject parent)
        {
            if (this.Parent != parent)
            {
                if (this.Parent != null) this.Parent.Children.Remove(this);

                this.Parent = parent;
                if (this.Parent != null) this.Parent.Children.Add(this);
            }
        }

        //Add a child to this object
        public void AddChild(LevelObject child)
        {
            if (child.Parent != null) child.Parent.Children.Remove(child);

            child.Parent = this;
            Children.Add(child);
        }

        //Completely removes a child from its parent and puts it in the level
        public void RemoveChildComplete(LevelObject child, Level level)
        {
            if (HasChild(child) == true)
            {
                child.Parent = null;
                Children.Remove(child);
                level.AddEnemy(child);
            }
        }

        //Remove a child from this object
        public void RemoveChild(LevelObject child)
        {
            if (HasChild(child) == true)
            {
                //If we have a parent, add the child to the parent
                if (this.Parent != null) this.Parent.AddChild(child);
                else
                {
                    child.Parent = null;
                    Children.Remove(child);
                }
            }
        }

        //Remove a child from this object by index
        public void RemoveChild(int index)
        {
            //Make sure the index isn't out of bounds
            if (index >= 0 && index < Children.Count)
            {
                //If we have a parent, add the child to the parent
                if (this.Parent != null) this.Parent.AddChild(Children[index]);
                else
                {
                    Children[index].Parent = null;
                    Children.RemoveAt(index);
                }
            }
        }

        //Get a child by index
        public LevelObject GetChild(int index)
        {
            LevelObject child = null;

            //Make sure the index isn't out of bounds
            if (index >= 0 && index < Children.Count)
                child = Children[index];
            
            return child;
        }

        //Get all the children this object has
        public List<LevelObject> GetChildren
        {
            get { return Children; }
        }

        //Check if an object is a child of this object
        public bool HasChild(LevelObject child)
        {
            return (child.IsChildOf(this));
        }

        //Check if this level object has any children
        public bool HasChildren
        {
            get { return (Children.Count != 0); }
        }

        //Check if this object is a child of a specified parent
        public bool IsChildOf(LevelObject parent)
        {
            return (this.Parent == parent);
        }

        //Set the active status of the object
        public void SetActive(bool active)
        {
            Active = active;
        }

        //Checks if the object can get hit
        public virtual bool CanGetHit(Rectangle rect)
        {
            return (hurtbox == null || (hurtbox.CanBeHit(rect) == true && IsInvincible == false && IsDying == false && IsDead == false));
        }

        //Gets the X position to stop the object in front of the player's castle, taking the Y position into account
        //Higher Y positions move slightly further to the right
        public int StopAtCastle(Vector2 playerpos, Vector2 animsize, int Range)
        {
            //The base position to stop; 13 is the amount of X space between the player position and the entrance to the gate
            int stop = (int)playerpos.X - (int)animsize.X - Range + 13;

            //float ypos = Position.Y;
            //if (Position.Y < (playerpos.Y + Player.GateStart))
            //    ypos = Position.Y - playerpos.Y;

            //Based on the Y position of the object, add more to the X stop position
            int playerentrance = (int)((Position.Y - playerpos.Y) + animsize.Y);

            stop += (playerentrance - Player.GateStart);

            return stop;
        }

        //Gives gold to the player
        public virtual void GrantGold(Level level, bool killedbyplayer)
        {

        }

        //Uses invincibility
        public virtual void UseInvincibility()
        {
            InvincibilityFade.RestartFade();
            PrevInvincibility = (Game1.ActiveTime + InvincibilityLength);
        }

        //Ends an object's invincibility
        public void EndInvincibility()
        {
            PrevInvincibility = 0;
        }

        //Kill the object and mark it as dead
        public virtual void Die(Level level)
        {
            Dead = true;

            // Kill all of the object's children
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Die(level);
            }
        }

        //Move the object a specified amount
        public void Move(Vector2 moveamount)
        {
            Position += moveamount;

            //Move the children with the parent
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Move(moveamount);
            }
        }

        public virtual void Update(Level level)
        {
            //Update children
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].IsDead == false)
                    Children[i].Update(level);
                else
                {
                    Children.RemoveAt(i);
                    i--;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Draw children
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].IsDead == false)
                    Children[i].Draw(spriteBatch);
            }
        }
    }
}
