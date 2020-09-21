using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class RefFilename
    {
        static string[] AcceptedFileExtensions = new string[] {".txt", ".yml"};
        public bool IsBase { get; set; }
        string Filename { get; set; }
        public bool IsDirectory
        {

            get
            {
                string filename = Filename;
                if (AcceptedFileExtensions.Any(a => filename.ToLower().EndsWith(a)))
                    return false;

                return true;
            }

        }
        public bool IsFile
        {

            get { return !IsDirectory; }

        }
        public bool Exists { 
            get
            {
                if (IsDirectory)
                    return Directory.Exists(ToFullFilename());

                return File.Exists(ToFullFilename());
            }
        }

        public string Name { 
            get
            {
                string f = Filename;
                if (f.EndsWith("/"))
                    f = f.Substring(0, f.Length - 1);

                if (f.Contains("/"))
                    return f.Substring(f.LastIndexOf("/") + 1);

                return f;
            }

        }

        public string Extension
        {
            get
            {
                if (IsDirectory)
                    return null;
                return Name.Substring(Name.LastIndexOf("."));
            }
        }

        public override int GetHashCode()
        {
            return ToRelativeFilename().GetHashCode() + (IsBase ? (1000000000) : 0);
        }

        public override bool Equals(object obj)
        {
            if(obj is RefFilename)
            {
                var ff = obj as RefFilename;

                if (ff.IsBase == IsBase && Filename == ff.Filename)
                    return true;
            }

            return false;
        }

        public static bool operator ==(RefFilename obj1, RefFilename ff)
        {
            if (((object)ff) == null)
                return ((object)obj1) == null;

            return (ff.IsBase == obj1.IsBase && obj1.Filename == ff.Filename);
        }

        // this is second one '!='
        public static bool operator !=(RefFilename obj1, RefFilename ff)
        {
            if (((object)ff) == null)
                return ((object)obj1) != null;
            return !(ff.IsBase == obj1.IsBase && obj1.Filename == ff.Filename);
        }


        public RefFilename(string filename)
        {
            Filename = filename;
            Filename = Filename.Replace("\\", "/");

            if (IsDirectory && !Filename.EndsWith("/"))
                Filename += "/";

            if (Filename.Contains(Globals.CK3Path))
                IsBase = true;
            else if (Core.Instance.ModCK3Library != null && Core.Instance.ModCK3Library.Name != null &&
                     Filename.Contains(Core.Instance.ModCK3Library.Path))
                IsBase = false;
            else
                throw new InvalidDataException("Path passed that doesn't fit in either mod or base directories.");


            Filename = Filename.Replace("//", "/");

            Filename = Filename.Replace(Globals.CK3Path, "");
            if (Core.Instance.ModCK3Library != null && Core.Instance.ModCK3Library.Name != null)
                Filename = Filename.Replace(Core.Instance.ModCK3Library.Path, "");

       

        }
        public RefFilename(string filename, bool isBase)
        {
            IsBase = isBase;
            Filename = filename;


            if (IsDirectory && !Filename.EndsWith("/"))
                Filename += "/";

            Filename = Filename.Replace("//", "/");

            Filename = Filename.Replace("\\", "/");
            Filename = Filename.Replace(Globals.CK3Path, "");
            if (Core.Instance.ModCK3Library != null && Core.Instance.ModCK3Library.Name != null)
                Filename = Filename.Replace(Core.Instance.ModCK3Library.Path, "");

            
        }

        public RefFilename[] GetDirectories()
        {
            if (!IsDirectory)
                throw new InvalidDataException("Is not a directory");

            var dirs = Directory.GetDirectories(ToFullWindowsFilename());

            List<RefFilename> f = new List<RefFilename>();
            foreach (var dir in dirs)
            {
                f.Add(new RefFilename(dir));
            }

            return f.ToArray();
        }
        public RefFilename[] GetFiles()
        {
            if (!IsDirectory)
                throw new InvalidDataException("Is not a directory");

            var dirs = Directory.GetFiles(ToFullWindowsFilename());

            List<RefFilename> f = new List<RefFilename>();
            foreach (var dir in dirs)
            {
                f.Add(new RefFilename(dir));
            }

            return f.ToArray();
        }

        public string ToFullFilename()
        {
            if (IsBase)
                return Globals.CK3Path + Filename;

            return Core.Instance.ModCK3Library.Path + Filename;
        }
        public string ToRelativeFilename()
        {
            return Filename;
        }
        public string ToFullWindowsFilename()
        {
            return ToFullFilename().Replace("/", "\\");
        }

        public string ToRelativeDirectoryOfFile()
        {
            if (IsDirectory)
                return ToRelativeFilename();

            return ToRelativeFilename().Replace(Name, "");
        }
        public string ToFullDirectoryOfFile()
        {
            if (IsDirectory)
                return ToFullFilename();

            return ToFullFilename().Replace(Name, "");
        }

        public RefFilename ChangeExtension(string extension)
        {
            string path = ToRelativeDirectoryOfFile();

            string ex = path + ToNameWithoutExtension();

            if (!extension.StartsWith("."))
                return new RefFilename(ex + "." + extension, IsBase);
            return new RefFilename(ex + extension, IsBase);
        }

        private string ToNameWithoutExtension()
        {
            string name = Name;

            return name.Replace(Extension, "");

        }

        public RefFilename Append(string sub)
        {
            if(!IsDirectory)
                throw new InvalidDataException("Cannot append to a filename");

            string relative = ToRelativeFilename();

            sub = sub.Replace("\\", "/");

            string newDir = relative + sub;

            return new RefFilename(newDir, IsBase);
        }

        public RefFilename RemoveFromPath(string pathPart)
        {
            return new RefFilename(ToRelativeFilename().Replace(pathPart, ""), IsBase);
        }

        public string[] SplitDirectories()
        {
            return ToRelativeFilename().Split('/');
        }

        public RefFilename ToBase()
        {
            return new RefFilename(Filename, true);
        }
        public RefFilename ToMod()
        {
            return new RefFilename(Filename, false);
        }

        public RefFilename SubDir(string s)
        {
            if(!IsDirectory)
                throw new InvalidDataException("Cannot subdir a file");

            return new RefFilename(Filename + s + "/", IsBase);
        }

        public RefFilename AsFileInDirectory(string file)
        {
            if (!IsDirectory)
                throw new InvalidDataException("Cannot add file to a file");

            return new RefFilename(Filename + file, IsBase);

        }
    }
}
