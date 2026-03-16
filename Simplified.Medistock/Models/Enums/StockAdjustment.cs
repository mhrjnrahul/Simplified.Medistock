namespace Simplified.Medistock.Models.Enums
{
    public enum AdjustmentType
    {
        Addition,    // Adding stock manually
        Reduction,   // Removing stock manually
        Correction,  // Fixing a count error
        Return,      // Customer returned item
        Damaged,     // Stock damaged
        Expired      // Stock expired and removed
    }
}