using System;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Actions.Base;

namespace UtilityAI_Base
{
    public enum UtilityActionType
    {
        Picker,
        Atomic
    }

    public sealed class UtilityPick : IEquatable<UtilityPick>
    {
        public readonly AbstractUtilityAction UtilityAction = null;
        public float Score = 0f;
        public readonly int SelectorIdx;
        public readonly UtilityActionType ActionType;

        public UtilityPick(AbstractUtilityAction action, float score, int selectorIdx = -1) {
            UtilityAction = action;
            Score = score;
            SelectorIdx = selectorIdx;
            if (action != null) {
                ActionType = action is AtomicUtilityAction ? UtilityActionType.Atomic : UtilityActionType.Picker;
            }
        }

        public bool Equals(UtilityPick other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(UtilityAction, other.UtilityAction) && ActionType == other.ActionType;
        }

        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || obj is UtilityPick other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return ((UtilityAction != null ? UtilityAction.GetHashCode() : 0) * 397) ^ (int) ActionType;
            }
        }

        public static bool operator ==(UtilityPick left, UtilityPick right) {
            return Equals(left, right);
        }

        public static bool operator !=(UtilityPick left, UtilityPick right) {
            return !Equals(left, right);
        }
    }
}