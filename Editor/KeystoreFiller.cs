#if UNITY_EDITOR 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Build1.UnityAndroidKeystoreFiller.Editor
{
    // TODO: Editor info window
    
    [InitializeOnLoad]
    internal static class KeystoreFiller
    {
        static KeystoreFiller()
        {
            const bool log = true;
            
            var keystorePath = GetKeystorePath();
            if (keystorePath == null)
            {
                if (log)
                    Debug.LogWarning("Keystore Filler: Keystore file not found");
                return;    
            }

            if (string.IsNullOrWhiteSpace(PlayerSettings.Android.keystoreName))
            {
                PlayerSettings.Android.keystoreName = keystorePath;
                
                if (log)
                    Debug.Log("Keystore Filler: Keystore set");
            }

            var passwordFilePath = keystorePath + ".txt";
            if (!File.Exists(passwordFilePath))
            {
                if (log)
                    Debug.LogWarning("Keystore Filler: Keystore password file not found");
                return;
            }

            var password = File.ReadAllText(passwordFilePath);
            
            PlayerSettings.Android.keystorePass = password;
            
            //PlayerSettings.Android.keyaliasName = password; // ???
            PlayerSettings.Android.keyaliasPass = password;
            
            if (log)
                Debug.LogWarning("Keystore Filler: All set");
        }

        private static string GetKeystorePath()
        {
            var rootPath = Path.Combine(Application.dataPath, "../");
            var ext = new List<string> { "keystore" };
            var files = Directory
                       .EnumerateFiles(rootPath, "*.*", SearchOption.AllDirectories)
                       .Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));
            return files.FirstOrDefault();
        }
    }
}

#endif