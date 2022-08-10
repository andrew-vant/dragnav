using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

namespace DraggableControls
{
    class DASettings : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "Draggable Controls"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Draggable Controls"; } }
        public override string DisplaySection { get { return "Draggable Controls"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return false; } }


        [GameParameters.CustomParameterUI("Allow Navball to be moved vertically")]
        public bool allowNavVertical = true;


        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            return true;
        }
        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            return true;
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }

    }
}
