using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    public enum Direction
    {
        Right, Left
    };

    //A base class for objects that exist in each level; this includes the Player Castle and Enemies
    //Hitboxes, hurtboxes, and the like are added as children, but references to them are kept
    public abstract class LevelObject
    {
        //Whether the object is active or not
        protected bool Active;

        //Whether the object is dead or not and should be removed
        protected bool Dead;

        //The position of the object
        protected Vector2 Position;

        //The direction the object is facing; defaults to Right
        protected Direction DirectionFacing;

        //A potential hitbox the object can have
        protected Hitbox hitbox;

        //Hurtbox of the object
        protected Hurtbox hurtbox;

        //The animation of the object
        protected Animation Animation;

        //The parent of the level object
        protected LevelObject Parent;

        //Children of the level object
        protected List<LevelObject> Children;

        public LevelObject()
        {
            Position = Vector2.Zero;
            DirectionFacing = Direction.Right;

            Children = new List<LevelObject>();
        }

        public bool IsActive
        {
            get { return Active; }
        }

        public bool IsDead
        {
            get { return Dead; }
        }

        //Get the position of the object
        public Vector2 GetPosition
        {
            get { return Position; }
        }

        //Get the direction the object is facing
        public Direction GetDirection
        {
            get { return DirectionFacing; }
        }

        //Sets the hitbox of the object
        public void SetHitbox(int width, int height)
        {
            hitbox = new Hitbox(Position, width, height);
            AddChild(hitbox);
        }

        //Sets the hurtbox of the object
        public void SetHurtbox(int width, int height)
        {
            hurtbox = new Hurtbox(Position, width, height);
            AddChild(hurtbox);
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

        //Kill the object and mark it as dead
        public void Die()
        {
            Dead = true;

            // Kill all of the object's children
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Die();
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

        public virtual void Update()
        {
            //Update children
            for (int i = 0; i < Children.Count; i++)
                Children[i].Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Draw children
            for (int i = 0; i < Children.Count; i++)
                Children[i].Draw(spriteBatch);
        }
    }
}
