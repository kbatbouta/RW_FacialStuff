﻿using FacialStuff.AnimatorWindows;
using Verse;

namespace FacialStuff.Animator
{
    public class BodyAnimator
    {
        // public float cyclePercent = 0;

        // public bool Finished;
        private readonly Pawn _pawn;

        private readonly CompBodyAnimator _compAnim;

        public BodyAnimator(Pawn p, CompBodyAnimator compAnim)
        {
            this._pawn          = p;
            this._compAnim = compAnim;
        }

        // Verse.PawnTweener

        public bool IsPosing(out float movedPercent)
        {
            movedPercent = 0f;

            if (this._compAnim.AnimatorPoseOpen)
            {
                movedPercent = MainTabWindow_BaseAnimator.AnimationPercent;
                return true;
            }

            return false;
        }

        public void AnimatorTick()
        {
            // if (this.pawn.pather.Moving)
            // {
            // float left = this.pawn.pather.nextCellCostLeft;
            // float total = this.pawn.pather.nextCellCostTotal;
            // this.cyclePercent = 1f - left / total;
            // this.Finished = false;
            // }
            // else
            // {
            // this.Finished = true;
            // }
        }
    }
}