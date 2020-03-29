
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.IO;

[StructLayout(LayoutKind.Sequential)]
public class SaveInfo
{
    //Score
    [MarshalAs(UnmanagedType.AsAny, SizeConst = 4)]
    public float highscore;
}

public class BinarySaveLoad : MonoBehaviour
{

    /// <summary>
    /// Saves a struct to a Binary file
    /// </summary>
    /// <param name="DataToSave">struct to save, is passed in as a object</param>
    /// <param name="Filepath">File path after Apllication.datapath</param>
    /// <param name="fileName">What to name the File</param>
    /// 
    public static void MarshalData(object DataToSave, string Filepath, string fileName)
    {
        try
        {
            Byte[] buffer = new byte[Marshal.SizeOf(DataToSave)]; //temporary buffer to store DataToSave as raw bytes

            IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(DataToSave)); //create and allocate pointer with enought space to store buffer
            Marshal.StructureToPtr(DataToSave, pnt, false);
            Marshal.Copy(pnt, buffer, 0, Marshal.SizeOf(DataToSave)); //copy Pointer memory to buffer memory

            Directory.CreateDirectory(Application.dataPath + Filepath); //check/create save folder location
                                                                        //string[] filenames = Directory.GetFiles( Application.dataPath + Filepath); // get all file in save location                                                                                 //string ToName = (fileName + (filenames.Length + 1)); //if more than 1 save exists create new save with next number of allready existing files
            FileStream SaveDataFile = new FileStream(Application.dataPath + Filepath + fileName + ".bin", FileMode.OpenOrCreate,
                                                                                                            FileAccess.ReadWrite,
                                                                                                            FileShare.None); ; //create Data stream 


            //SaveDataFile = File.Create( Application.dataPath + Filepath + fileName + ".bin");//give steam loation to save data aswell as create file
            BinaryWriter Bwrite = new BinaryWriter(SaveDataFile); //create a new binary writer to write data
            Bwrite.Write(buffer); // write data to file

            Bwrite.Close(); //Close binaryWriter
            SaveDataFile.Close(); //Close down Data stream
            Marshal.FreeHGlobal(pnt); //Free memory from pointer
        }
        catch
        {
            Debug.Log("ERROR IN SAVEING");
        }
    }

    /// <summary>
    /// Check to see if a Binary file with NAME exisit at in location FilePath, if not, creates one
    /// </summary>
    /// <param name="Filepath">Path to folder to check</param>
    /// <param name="fileName">name of the file to check</param>
    /// <returns></returns>
    public static bool CheckFileExists(string Filepath, string fileName)
    {
        if (!File.Exists(Application.dataPath + Filepath + fileName + ".bin"))
        {
            Directory.CreateDirectory(Application.dataPath + Filepath);
            File.Create(Application.dataPath + Filepath + fileName + ".bin");
            Debug.Log("Settings file does not exist/cant be found");
            return false;
        }
        Debug.Log("Settings file exist/found");
        return true;
    }

    /// <summary>
    /// Same as MarshalData exepet will return if the file does not all read exist,DO NOT USE BROKEN( will Reset file) - oliver
    /// </summary>
    /// <param name="DataToSave"></param>
    /// <param name="Filepath"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool MarshalDataCheck(object DataToSave, string Filepath, string fileName)
    {

        if (!File.Exists(Application.dataPath + Filepath + fileName + ".bin"))
        {
            return false;
        }

        Byte[] buffer = new byte[Marshal.SizeOf(DataToSave)]; //temporary buffer to store DataToSave as raw bytes

        IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(DataToSave)); //create and allocate pointer with enought space to store buffer
        Marshal.StructureToPtr(DataToSave, pnt, false);
        Marshal.Copy(pnt, buffer, 0, Marshal.SizeOf(DataToSave)); //copy Pointer memory to buffer memory

        //Directory.CreateDirectory( Application.dataPath + Filepath); //check/create save folder location
        //string[] filenames = Directory.GetFiles( Application.dataPath + Filepath); // get all file in save location
        //string ToName = (fileName + (filenames.Length + 1)); //if more than 1 save exists create new save with next number of allready existing files
        FileStream SaveDataFile; //create Data stream 

        SaveDataFile = File.Create(Application.dataPath + Filepath + fileName + ".bin");//give steam loation to save data aswell as create file
        BinaryWriter Bwrite = new BinaryWriter(SaveDataFile); //create a new binary writer to write data
        Bwrite.Write(buffer); // write data to file

        Bwrite.Close(); //Close binaryWriter
        SaveDataFile.Close(); //Close down Data stream
        Marshal.FreeHGlobal(pnt); //Free memory from pointer

        return true;

    }


    /// <summary>
    /// Load data from binaray file back in to there respective structure
    /// </summary>
    /// <param name="Data">Passed in memory of structure as an object</param>
    /// <param name="Filepath">File path after Apllication.datapath</param>
    /// <param name="fileName">What the file is name</param>
    /// <param name="DataType">the structure type used for sizing, use GetType()</param>
    public static void UnMarshalData(ref object Data, string Filepath, string fileName, Type DataType)
    {
        FileStream Back = File.OpenRead(Application.dataPath + Filepath + fileName + ".bin"); //create a datastream from desired file to load
        BinaryReader Bread = new BinaryReader(Back); //create a binary reader to read binary file bytes

        Byte[] BackBuffer = new byte[Back.Length];// buffer to store binary savefile
        BackBuffer = Bread.ReadBytes((int)Back.Length);

        //Replaced with ref object
        //SaveFile readback; //structure to repop with data from binary file

        IntPtr pnt2 = Marshal.AllocHGlobal((int)Back.Length);//create and allocate pointer with enought space to store buffer
        Marshal.Copy(BackBuffer, 0, pnt2, (int)Back.Length); //copy buffer to pointer memory

        Data = Marshal.PtrToStructure(pnt2, DataType);// convert data back to structure 

        Bread.Close(); // close binary Reader
        Marshal.FreeHGlobal(pnt2);//free memory from pointer
    }
}