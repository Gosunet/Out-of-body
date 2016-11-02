using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Xml;
using System.Collections.Generic;
using System.Collections;

namespace ManageLog
{

    public class FileLog
    {

        private string SEPARATOR = "\t";
        private float[] _modelSrcValues;
        private float[] _modelDstValues;
        private float[] _differenceModels;
        private XmlDocument _xmlModel;
        private string _dateSelectedAvatar;
        GameObject src;
        GameObject dst;

        public FileLog()
        {
            // Get name of the current model
            string[] modelName = PlayerPrefs.GetString(Utils.PREFS_MODEL).Split(';');
            loadXMLFromAssest();
            if (!modelName[0].Equals(""))
            {
                _modelSrcValues = readModelsValue(modelName[0].Split('/')[2]);  //Patient model save at the index 0 of the "modelName" array
                _modelDstValues = readModelsValue(modelName[1].Split('/')[2]);  //Psychologue model save at the index 0 of the "modelName" array
                _differenceModels = calculEcart(_modelSrcValues, _modelDstValues);
            }
            
        }

        /// <summary>
        /// Create the log file of selected avatar
        /// </summary>
        public void createAvatarLogFile(int dif, GameObject[] goModel, int avatarIndex)
        {
            string gender;
            string modelSrcvalue;
            string modelDstvalue;
            string modelDiffvalue;

            src = goModel[avatarIndex];
            dst = selectOtherAvatar(src, dif, goModel);

            // Get gender of the current patient
            if (PlayerPrefs.GetInt(Utils.PREFS_AVATAR_GENDER) == 0) gender = "HOMME";
            else gender = "FEMME";

            // Get name of the current model
            string[] modelName = PlayerPrefs.GetString(Utils.PREFS_MODEL).Split(';');
            if (!modelName[0].Equals(""))
            {
                _modelSrcValues = readModelsValue(modelName[0].Split('/')[2]);  //Patient model save at the index 0 of the "modelName" array
                _modelDstValues = readModelsValue(modelName[1].Split('/')[2]);  //Psychologue model save at the index 0 of the "modelName" array
                _differenceModels = calculEcart(_modelSrcValues, _modelDstValues);
            }


            //Extract data from gender model (patient + psychologue + difference)
            if (_modelSrcValues != null)
            {
                modelSrcvalue = _modelSrcValues[0].ToString() + SEPARATOR + _modelSrcValues[1].ToString() + SEPARATOR + _modelSrcValues[2].ToString();
                modelDstvalue = _modelDstValues[0].ToString() + SEPARATOR + _modelDstValues[1].ToString() + SEPARATOR + _modelDstValues[2].ToString();
                modelDiffvalue = _differenceModels[0].ToString() + SEPARATOR + _differenceModels[1].ToString() + SEPARATOR + _differenceModels[2].ToString();
            }
            else
            {
                modelSrcvalue = "0" + SEPARATOR + "0 " + SEPARATOR + "0";
                modelDstvalue = "0" + SEPARATOR + "0 " + SEPARATOR + "0";
                modelDiffvalue = "0" + SEPARATOR + "0 " + SEPARATOR + "0";
            }

            string fileName = PlayerPrefs.GetString(Utils.PREFS_EXPERIMENT_PATH_FOLDER) + "/" + FilesConst.FILENAME_LOG_AVATAR_SELECT;
            Debug.Log(fileName);
            StreamWriter fileWritter = null;


            if (!File.Exists(fileName))
            {
                fileWritter = new StreamWriter(fileName, true); //true: ajouter a la fin du fichier 
                fileWritter.WriteLine("Date_Heure" + SEPARATOR +
                                       "Sexe" + SEPARATOR +
                                       "Choix patient" + SEPARATOR + SEPARATOR + SEPARATOR +
                                       "Choix experimentateur" + SEPARATOR + SEPARATOR + SEPARATOR +
                                       "Difference corpulence" + SEPARATOR + SEPARATOR + SEPARATOR +
                                       "Facteur de reduction");
            }
            else
            {
                fileWritter = new StreamWriter(fileName, true);
            }

            _dateSelectedAvatar = System.DateTime.Now.ToString().Replace("/", "-").Replace(":", "-");
            fileWritter.WriteLine(_dateSelectedAvatar + SEPARATOR + 
                                    gender + SEPARATOR + 
                                    modelSrcvalue + SEPARATOR +
                                    modelDstvalue + SEPARATOR + 
                                    modelDiffvalue + SEPARATOR + 
                                    dif);

            fileWritter.Close();
        }


        /// <summary>
        /// Creates the experimentation result file.
        /// </summary>
        /// <param name="dir">directory</param>
        /// <param name="username">Name of the patient</param>
        /// <param name="numEx">Number exercice</param>
        /// <param name="ordreOuverture">doors values</param>
        /// <param name="answers">answers patient</param>
        public void createResultFile(string dir, string username, int numEx, List<Measure> ordreOverture, List<bool> answers)
        {
            string modelSrcvalue;
            string modelDstvalue;
            string modelDiffvalue;

            if (_modelSrcValues != null)
            {
                modelSrcvalue = _modelSrcValues[0].ToString() + SEPARATOR + _modelSrcValues[1].ToString() + SEPARATOR + _modelSrcValues[2].ToString();
                modelDstvalue = _modelDstValues[0].ToString() + SEPARATOR + _modelDstValues[1].ToString() + SEPARATOR + _modelDstValues[2].ToString();
                modelDiffvalue = _differenceModels[0].ToString() + SEPARATOR + _differenceModels[1].ToString() + SEPARATOR + _differenceModels[2].ToString();
            }
            else
            {
                modelSrcvalue = "0" + SEPARATOR + "0" + SEPARATOR + "0";
                modelDstvalue = "0" + SEPARATOR + "0" + SEPARATOR + "0";
                modelDiffvalue = "0" + SEPARATOR + "0" + SEPARATOR + "0";
            }

            string filename = Path.Combine(dir, username + ".txt");
            StreamWriter file = null;

            if (!File.Exists(filename))
            {
                file = new StreamWriter(filename, true);
                file.WriteLine("N° Exercice" + SEPARATOR +
                                "N° Porte" + SEPARATOR +
                                "Condition" + SEPARATOR +
                                "Type de porte" + SEPARATOR +
                                "Largeur de porte" + SEPARATOR +
                                "Hauteur de porte" + SEPARATOR +
                                "Reponse" + SEPARATOR +
                                "Corpulence utilisateur" + SEPARATOR + SEPARATOR + SEPARATOR +
                                "Corpulence docteur" + SEPARATOR + SEPARATOR + SEPARATOR +
                                "Difference de corpulence" + SEPARATOR + SEPARATOR + SEPARATOR +
                                "Temps de reponse");
            }
            else
            {
                file = new StreamWriter(filename, true);
            }

            int condition = PlayerPrefs.GetInt(Utils.PREFS_CONDITION);
            string doors = PlayerPrefs.GetString(Utils.PREFS_DOORS);
            int doorType;

            if (doors.Equals(Utils.BOTTOM_DOORS)) doorType = FilesConst.BOTTOM_DOOR;
            else if (doors.Equals(Utils.TOP_DOORS)) doorType = FilesConst.TOP_DOOR;
            else doorType = FilesConst.FULL_DOOR;


            for (int nbDoor = 0; nbDoor < ordreOverture.Count; nbDoor++)
            {
                file.WriteLine(numEx + SEPARATOR +
                                (nbDoor + 1).ToString() + SEPARATOR +
                                condition.ToString() + SEPARATOR +
                                doorType.ToString() + SEPARATOR +
                                ordreOverture[nbDoor].width.ToString() + SEPARATOR +
                                ordreOverture[nbDoor].height.ToString() + SEPARATOR +
                                (answers[nbDoor] == true ? "1" : "0") + SEPARATOR +
                                modelSrcvalue + SEPARATOR +
                                modelDstvalue + SEPARATOR +
                                modelDiffvalue + SEPARATOR +
                                ordreOverture[nbDoor].time.ToString());
            }
            file.Close();
        }


        /// <summary>
        /// Creates the experimentation result file.
        /// </summary>
        /// <param name="numEx">Number exercice</param>
        public void createConfigFile(int numEx)
        {
            string filename = Path.Combine(PlayerPrefs.GetString(Utils.PREFS_EXPERIMENT_PATH_FOLDER), FilesConst.FILENAME_CONFIG_EXERCICE + ".txt");
            string config = PlayerPrefs.GetString(Utils.PREFS_PARAM_DOORS);

            string[] parameters = config.Split('_');
            string nb_largeur = parameters[1];
            string min_largeur = parameters[2];
            string max_largeur = parameters[3];
            string nb_hauteur = parameters[4];
            string min_hauteur = parameters[5];
            string max_hauteur = parameters[6];
            string nb_repetition = parameters[0];

            string doors = PlayerPrefs.GetString(Utils.PREFS_DOORS);
            int doorType;

            if (doors.Equals(Utils.BOTTOM_DOORS)) doorType = FilesConst.BOTTOM_DOOR;
            else if (doors.Equals(Utils.TOP_DOORS)) doorType = FilesConst.TOP_DOOR;
            else doorType = FilesConst.FULL_DOOR;

            StreamWriter file = null;

            if (!File.Exists(filename))
            {
                file = new StreamWriter(filename, true);
                file.WriteLine("N° Exercice" + SEPARATOR +
                                "Type exercice" + SEPARATOR +
                                "Nb largeur" + SEPARATOR +
                                "Min largeur" + SEPARATOR +
                                "Nb hauteur" + SEPARATOR +
                                "Min hauteur" + SEPARATOR +
                                "Max hauteur" + SEPARATOR +
                                "Nb repetition"
                                );
            }
            else
            {
                file = new StreamWriter(filename, true);
            }

            file.WriteLine(numEx + SEPARATOR +
                            doorType + SEPARATOR +
                            nb_largeur + SEPARATOR +
                            min_largeur + SEPARATOR +
                            max_largeur + SEPARATOR +
                            nb_hauteur + SEPARATOR +
                            min_hauteur + SEPARATOR +
                            max_hauteur + SEPARATOR +
                            nb_repetition);

            file.Close();

        }


        /// <summary>
        /// Reads the models value on the xml.
        /// </summary>
        /// <returns>The models value.</returns>
        /// <param name="name">Name.</param>
        public float[] readModelsValue(string name)
        {
            string nameNode = "", waist = "", hips = "", chest = "";
            foreach (XmlElement node in _xmlModel.SelectNodes("Models/Model"))
            {
                nameNode = node.GetAttribute("name");
                if (nameNode.Equals(name))
                {
                    XmlNode waistNode = node.SelectSingleNode("Waist");
                    waist = waistNode.InnerText;
                    XmlNode chestNode = node.SelectSingleNode("Chest");
                    chest = chestNode.InnerText;
                    XmlNode hipsNode = node.SelectSingleNode("Hips");
                    hips = hipsNode.InnerText;
                }
            }
            float[] values = new float[3];
            values[0] = float.Parse(waist);
            values[1] = float.Parse(chest);
            values[2] = float.Parse(hips);
            return values;
        }


        /// <summary>
        /// calculate the difference bewteen patient and psycholog model.
        /// </summary>
        /// <returns>The ecart.</returns>
        /// <param name="src">Source.</param>
        /// <param name="dst">Dst.</param>
        public float[] calculEcart(float[] src, float[] dst)
        {
            float[] resultat = new float[3];
            for (int i = 0; i < resultat.Length; i++)
                resultat[i] = (dst[i] - src[i]) / src[i] * 100;
            return resultat;
        }


        /// <summary>
        /// Loads the XML from assest.
        /// </summary>
        public void loadXMLFromAssest()
        {
            _xmlModel = new XmlDocument();
            TextAsset textXml = (TextAsset)Resources.Load("Models", typeof(TextAsset));
            _xmlModel.LoadXml(textXml.text);
        }


        GameObject selectOtherAvatar(GameObject avatar, int difference, GameObject[] goModel)
        {
            string[] weight = { "HC", "LHC", "MC", "HLC", "LC" };
            string[] muscle = { "HM", "LHM", "MM", "HLM", "LM" };

            string suffixeAvatar = "";

            for (int i = 0; i < weight.Length; i++)
            {
                for (int j = 0; j < muscle.Length; j++)
                {
                    if (avatar.name.Substring(avatar.name.LastIndexOf("ale") + 3).Equals(weight[i] + muscle[j]))
                    {
                        if (i + difference > weight.Length - 1)
                            suffixeAvatar = weight[weight.Length - 1];
                        else
                            suffixeAvatar = weight[i + difference];
                        if (j + difference > muscle.Length - 1)
                            suffixeAvatar += muscle[muscle.Length - 1];
                        else
                            suffixeAvatar += muscle[j + difference];
                    }
                }
            }
            foreach (GameObject character in goModel)
            {
                if (character.name.Substring(character.name.LastIndexOf("ale") + 3).Equals(suffixeAvatar))
                    return character;
            }
            return null;
        }


    }

    public class Measure
    {
        private float _width;
        private float _height;
        private double _time;

        public Measure(float width, float height)
        {
            _width = width;
            _height = height;
            _time = 0;
        }


        public float width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public float height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        public double time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = ((int)value) / 1000.0;
            }
        }
    }

}