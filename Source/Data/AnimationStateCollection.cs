using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class AnimationStateCollection : List<AnimationState>
    {
        public AnimationState Create(GameState gameState, TriggerState triggerState, IPlayer player) 
        {
            AnimationState animationState = new AnimationState();
            animationState.TriggerState = triggerState;
            animationState.FrameCurrent = 0;
            animationState.Animation = triggerState.Resource.Animations.GetAnimation(triggerState.Trigger.AnimationName);
            animationState.ResourceRoot = triggerState.ResourceRoot;
            animationState.Resource = triggerState.Resource;
            triggerState.AnimationState = animationState;
            if (triggerState.Trigger.Type == TriggerType.Factory)
                animationState.ResourceState = gameState.ResourceStates.Create(triggerState, animationState, player);
            this.Add(animationState);
            return (animationState);
        }

        public AnimationState Create(Resource game, Screen screen)
        {
            AnimationState animationState = AnimationStateCollection.CreateOnly(game, screen);
            if (animationState != null)
                this.Add(animationState);
            return (animationState);
        }

        public static AnimationState CreateOnly(Resource game, Screen screen)
        {
            if (string.IsNullOrEmpty(screen.AnimationName))
                return (null);
            AnimationState animationState = new AnimationState();
            animationState.FrameCurrent = 0;
            animationState.Animation = game.Animations.GetAnimation(screen.AnimationName);
            animationState.ResourceRoot = game;
            animationState.Resource = game;
            return (animationState);
        }

        public AnimationState Create(ResourceState resourceState, Spawn spawn) 
        {
            return (this.Create(resourceState, spawn, null));
        } 

        public AnimationState Create(ResourceState resourceState, Spawn spawn, Resource resourceRoot) 
        {
            AnimationState animationState = new AnimationState();
            animationState.FrameCurrent = 0;
            animationState.Animation = resourceState.Resource.Animations.GetAnimation(spawn.AnimationName);
            animationState.ResourceRoot = resourceRoot ?? resourceState.Resource;
            resourceState.AnimationState = animationState;
            animationState.Resource = resourceState.Resource;
            animationState.ResourceState = resourceState;
            this.Add(animationState);
            return (animationState);
        }

        public AnimationState Create(ResourceState resourceState, string animationName, Resource resourceRoot)
        {
            AnimationState animationState = new AnimationState();
            animationState.FrameCurrent = 0;
            animationState.Animation = resourceState.Resource.Animations.GetAnimation(animationName);
            animationState.ResourceRoot = resourceRoot ?? resourceState.Resource;
            resourceState.AnimationState = animationState;
            animationState.Resource = resourceState.Resource;
            animationState.ResourceState = resourceState;
            this.Add(animationState);
            return (animationState);
        }

        public AnimationState GetAnimationState(ResourceState resourceState) 
        {
            foreach (AnimationState animationState in this)
                if (animationState.ResourceState == resourceState)
                    return (animationState);
            return (null);
        }
    }
}
