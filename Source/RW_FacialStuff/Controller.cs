﻿namespace FacialStuff
{
    using RimWorld;

    using UnityEngine;

    using Verse;

    public class Controller : Mod
    {
        public Controller(ModContentPack content)
            : base(content)
        {
            settings = this.GetSettings<Settings>();
        }

        #region Fields

        public override string SettingsCategory()
        {
            return "Facial Stuff";
        }

        public static Settings settings;

        #endregion Fields

        #region Methods

        public override void WriteSettings()
        {
            if (settings != null)
            {
                settings.Write();
            }

            if (Current.ProgramState == ProgramState.Playing)
            {
                if (Find.ColonistBar != null)
                {
                    Find.ColonistBar.MarkColonistsDirty();
                }

                foreach (Pawn pawn in PawnsFinder.AllMapsAndWorld_Alive)
                {
                    if (pawn.RaceProps.Humanlike)
                    {
                        CompFace faceComp = pawn.TryGetComp<CompFace>();
                        if (faceComp != null)
                        {
                            // This will force the renderer to make "AllResolved" return false, if pawn is drawn
                            pawn.Drawer.renderer.graphics.nakedGraphic = null;
                        }
                    }
                }
            }
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }
    }

    #endregion Methods

    // Backup:
    // <MaleAverageOffsetX>0.04716981</MaleAverageOffsetX>
    // <MaleAverageOffsetY>0.01320755</MaleAverageOffsetY>
    // <MaleNarrowOffsetX>-0.0245283</MaleNarrowOffsetX>
    // <MaleNarrowOffsetY>0.03773585</MaleNarrowOffsetY>
    // <FemaleAverageOffsetX>0.03584905</FemaleAverageOffsetX>
    // <FemaleAverageOffsetY>0.04150943</FemaleAverageOffsetY>
    // <FemaleNarrowOffsetX>0.009433965</FemaleNarrowOffsetX>
    // <FemaleNarrowOffsetY>0.06981131</FemaleNarrowOffsetY>
    // <MaleAverageNormalOffsetX>0.02641509</MaleAverageNormalOffsetX>
    // <MaleAveragePointyOffsetX>0.02264151</MaleAveragePointyOffsetX>
    // <MaleAverageWideOffsetX>0.00754717</MaleAverageWideOffsetX>
    // <FemaleAverageNormalOffsetX>0.02452831</FemaleAverageNormalOffsetX>
    // <FemaleAveragePointyOffsetX>0.02452831</FemaleAveragePointyOffsetX>
    // <FemaleAverageWideOffsetX>0.04150943</FemaleAverageWideOffsetX>
}