using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GAWrap2.Steps
{
    public delegate void stepsChange();

    interface sInterface
    {
        /// <summary>
        /// Event for if steps are modified
        /// </summary>
        event stepsChange sChange;

        /// <summary>
        /// Get steps count
        /// </summary>
        int count { get; }
        
        /// <summary>
        /// Load steps from a file into memory
        /// </summary>
        /// <param name="file">Full file path with extension</param>
        void read(string file);

        /// <summary>
        /// Copy a new steps object and deallocate the old immediately
        /// </summary>
        /// <param name="s"></param>
        void copy(SControl s);

        /// <summary>
        /// Clean up steps data.
        /// </summary>
        void close();

        /// <summary>
        /// Handle if a step changes
        /// </summary>
        void onChange();

        /// <summary>
        /// Write steps in memory into a file
        /// </summary>
        /// <param name="file">Full file path with extension</param>
        void writeTo(string file);

        /// <summary>
        /// Write steps in memory into a file
        /// </summary>
        /// <param name="file">Full file path with extension</param>
        /// <param name="length">The number of steps to write to the file.</param>
        void writeTo(string file, int length);

        /// <summary>
        /// Modify an event
        /// </summary>
        /// <param name="sIndex">Steps index</param>
        /// <param name="eIndex">Events index</param>
        /// <param name="ev">New event</param>
        void modEvent(int sIndex, int eIndex, string ev);

        /// <summary>
        /// Remove a Step
        /// </summary>
        /// <param name="sIndex">Steps index</param>
        void remStep(int sIndex);

        /// <summary>
        /// Get a copy of a Step
        /// </summary>
        Step this[int i] { get; }
        
        /// <summary>
        /// Insert a Step
        /// </summary>
        /// <param name="sIndex">Steps index</param>
        /// <param name="s">Step to instert</param>
        void insert(int sIndex, Step s);

        /// <summary>
        /// Replace a bitmap (not currently implemented)
        /// </summary>
        void addBitmap(Bitmap bmp);

        /// <summary>
        /// Check if user input can be undone
        /// </summary>
        bool canUndo();

        /// <summary>
        /// Undo user input
        /// </summary>
        /// <returns>Index to step that was undone</returns>
        int undo();
    }
}
