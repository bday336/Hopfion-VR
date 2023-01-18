using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    // Information for the field grid
    public GameObject arrow;                            // Instance of arrow object
    static GameObject[] arrows;                         // Container for arrow objects present in the grid
    public int gridResolution = 3;                      // Number of arrows in each direction of grid - ie 5 x 5 x 5
    public float gridWidth = 2;                         // Width of grid (cube so same for all sides) - ie 2m x 2m x 2m

    // Information for field
    public static Vector3[] backgroundField;            // container for field values for all grid points
    public static float maxField = 0;                   // FUTURE use for spin color mapping
    public static float minField;                       // FUTURE use for spin color mapping
    public static bool spinColorToggle = false;         // FUTURE use for spin color mapping

    // Placeholder variables to simplify the field expressions
    public float px;
    public float py;
    public float pz;
    public float vecx;
    public float vecy;
    public float vecz;

    // Initialize the field grid in the scene
    void Start()
    {
        // Generate the grid
        Vector3[,,] grid = CreateGrid(gridResolution, gridWidth, transform.position);

        // Define container for all arrow objects in grid
        arrows = new GameObject[gridResolution * gridResolution * gridResolution];

        // Initialize field to be visualized by the grid and use to set arrow directions
        backgroundField = new Vector3[gridResolution * gridResolution * gridResolution];

        // Populate the grid with arrows (index allows for 1D selection of arrows in grid)
        int index = 0;
        for (int i = 0;  i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                for (int k = 0; k < grid.GetLength(0); k++)
                {
                    // Instantiate the arrow prefab oriented vertically. Arrow prefab originally oriented along
                    // the unity negative x axis but we want it to be oriented along the unity positive y axis
                    // which will now be our real world positive z axis.
                    arrows[index] = Instantiate(arrow, grid[i, j, k], Quaternion.AngleAxis(-90, Vector3.forward));

                    // Manually rotate since Unity is left-handed and we want to visualize our
                    // field in the typical right-handed coordinate convention
                    // Simple fix is to switch the y and z axes (in our mind) and apply the rotations
                    // we want to correctly orient.
                    px = arrows[index].transform.position[0];
                    py = arrows[index].transform.position[2];
                    pz = arrows[index].transform.position[1];

                    
                    vecx = 4 * (2 * px * pz - py * (px * px + py * py + pz * pz - 1)) / ((1 + px * px + py * py + pz * pz) * (1 + px * px + py * py + pz * pz));
                    vecy = 4 * (2 * py * pz + px * (px * px + py * py + pz * pz - 1)) / ((1 + px * px + py * py + pz * pz) * (1 + px * px + py * py + pz * pz));
                    vecz = 1 - 8 * (px * px + py * py) / ((1 + px * px + py * py + pz * pz) * (1 + px * px + py * py + pz * pz));
                    

                    /*
                    vecx = -py;
                    vecy = px;
                    vecz = 0;
                    */

                    backgroundField[index][0] = vecx;
                    backgroundField[index][1] = vecy;
                    backgroundField[index][2] = vecz;

                    // Rotates about the Unity axis in order of Z - X - Y to orient the arrows to point along given field
                    // Make sure the angles are in degrees and not radian
                    // In unity atan2(x,z) is angle between point at (x,0,z) and the z axis so order matters for arguments
                    arrows[index].transform.Rotate(0f,-Mathf.Atan2(vecy,vecx)*Mathf.Rad2Deg,-Mathf.Atan2(Mathf.Sqrt(Mathf.Pow(vecx,2f)+ Mathf.Pow(vecy, 2f)),vecz)*Mathf.Rad2Deg, Space.World);

                    // Color the arrows based on the polar orientation (currently the coloration is based on polar angle with zero being blue and pi being red)
                    arrows[index].GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.blue, Color.red, Mathf.Atan2(Mathf.Sqrt(Mathf.Pow(vecx, 2f) + Mathf.Pow(vecy, 2f)), vecz) / (Mathf.PI));

                    // Move the arrows for that they are centered 1.1 units above the ground so that the user
                    // can see the whole field
                    arrows[index].transform.position = arrows[index].transform.position + Vector3.up * 1.1f;

                    index++;
                }
            }
        }
        
    }
 
    void Update()
    {


    }

    // Method to generate grid to visualize field
    public static Vector3[,,] CreateGrid(int resolution, float width, Vector3 position)
    {
        Vector3[,,] grid = new Vector3[resolution, resolution, resolution];

        float spacing = (float) width / (resolution-1);

        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                for (int k = 0; k < resolution; k++)
                {
                    grid[i, j, k] = position + new Vector3(spacing * i - width/2, spacing * j - width / 2, spacing * k - width / 2);
                }
            }
        }

        return grid;
    }

}
