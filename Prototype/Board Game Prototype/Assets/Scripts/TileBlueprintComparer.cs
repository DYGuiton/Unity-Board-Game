using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TileBlueprintComparer : IComparable {

    //Default Comparer
    public int CompareTo(object obj) {
        throw new NotImplementedException();
    }

    //Methods to return non-default Comparers
    public static IComparer<TileBlueprint> sortDescendingXPosition() {
        return new DescendingXPosition();
    }

    //nested classes for non default Comparers
    private class DescendingXPosition : IComparer<TileBlueprint> {

        public int Compare(TileBlueprint first, TileBlueprint second) {
            if (first == null || second == null) {
                return 1;
            }
            else {

                //Compare two Tiles and return the leftmost one
                int compare_value = first.Location.x.CompareTo(second.Location.x);
                compare_value = compare_value * -1;

                //If the Tiles are in the same row, return the highest one
                if (compare_value == 0) {
                    compare_value = first.Location.z.CompareTo(second.Location.z);
                }

                return compare_value;
            }
        }
    }
}
