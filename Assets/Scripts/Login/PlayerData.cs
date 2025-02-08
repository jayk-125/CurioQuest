/* Author: Loh Shau Ern Shaun & Jonathan Low Jerome Enting
 * Date: 04/02/2025
 * Desc:
 * - Construct to contain player database details
 */
using System;
public class PlayerData
{
    // Data to be stored
    public string uid;
    public string username;
    public int highscore;

    // Initialize empty constructor
    public PlayerData()
    {

    }

    // Filling up the constructor
    public PlayerData(string uid, string username, int highscore)
    {
        this.uid = uid;
        this.username = username;
        this.highscore = highscore;
    }
}