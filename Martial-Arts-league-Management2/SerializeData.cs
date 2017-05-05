using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MartialArts
{

    class SerializeData
    {
        public Visual.VisualLeagueEvent.UndoStruct VisualObjects;

        public SerializeData(Visual.VisualLeagueEvent.UndoStruct Undostruct)
        {
            VisualObjects = Undostruct;
        }

        public SerializeData()
        {

        }

        public virtual bool Serialize()
        {
            if (VisualObjects._VisualBracketsList ==  null)
            {
                throw new Exception("you must use the oveloaded constructor and pass him Visual.VisualLeagueEvent.UndoStruct object in order to serialize");
            }

            try
            {
                // Serialize all visual contenders
                using (Stream stream = File.Open(MartialArts.BinaryFiles.SavedAllContsBinaryFilePath, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, VisualObjects.AllVisualContenders.Select(x => x.Contender).ToList());
                }
                // Serialize visual brackets
                using (Stream stream = File.Open(MartialArts.BinaryFiles.SavedVbBinaryFilePath, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, VisualObjects._VisualBracketsList.Select(x => x.Bracket).ToList());
                }
                // Serialize unplaced visual contenders
                using (Stream stream = File.Open(MartialArts.BinaryFiles.SavedUnplacedVcBinaryFilePath, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, VisualObjects._VisualUnplacedBracketsList.Select(x=> x.Contender).ToList());
                }

                // Serialize data from datagridview (dgvmain)
                using (Stream stream = File.Open(MartialArts.BinaryFiles.ContendersFilePath, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, VisualObjects.ContendersInsideDgvMain);
                }

                return true;
            }

            catch (Exception ex)
            {
                Helpers.ShowGenericPromtForm("התרחשה בעיה בשמירת הנתונים:" + Environment.NewLine + ex.Message.ToString());
                return false;
            }

        }


        /// <summary>
        ///  overide function to save only contenders (not visaul contenders) in case visual brackets did not create yet and there is only datagridview
        /// </summary>
        /// <param name="contenders">list of contenders</param>
        /// <returns></returns>
        public virtual bool Serialize(List<Contenders.Contender> contenders)
        {
          
            try
            {
                // Serialize all visual contenders
                using (Stream stream = File.Open(MartialArts.BinaryFiles.ContendersFilePath, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, contenders);
                }

                // serilize the other files with empty undo struct in order to know that only contenders where saved in the last time
                setSavedFilesToNull();

                return true;
            }



            catch (Exception ex)
            {
                Helpers.ShowGenericPromtForm("התרחשה בעיה בשמירת הנתונים:" + Environment.NewLine + ex.Message.ToString());
                return false;
            }

        }

        protected virtual void setSavedFilesToNull()
        {
            // Serialize all visual contenders
            using (Stream stream = File.Open(MartialArts.BinaryFiles.SavedAllContsBinaryFilePath, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, "");
            }
            // Serialize visual brackets
            using (Stream stream = File.Open(MartialArts.BinaryFiles.SavedVbBinaryFilePath, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, "");
            }
            // Serialize unplaced visual contenders
            using (Stream stream = File.Open(MartialArts.BinaryFiles.SavedUnplacedVcBinaryFilePath, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, "");
            }
        }

        public virtual BracketsBuilder DeSerialize(out bool IsOk)
        {
            BracketsBuilder savedData = new BracketsBuilder();

            try
            {

                // Serialize visual brackets
                using (Stream stream = File.Open(MartialArts.BinaryFiles.SavedVbBinaryFilePath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var temp = bformatter.Deserialize(stream);
                    savedData.BracketsList = temp as List<Bracket>;
                }

                // Serialize unplaced visual contenders
                using (Stream stream = File.Open(MartialArts.BinaryFiles.SavedUnplacedVcBinaryFilePath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var temp = bformatter.Deserialize(stream);
                    savedData.ContendersList = temp as List<Contenders.Contender>;
                }

                // Serialize data from datagridview
                using (Stream stream = File.Open(MartialArts.BinaryFiles.ContendersFilePath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var temp = bformatter.Deserialize(stream);
                    savedData.ContendersFromDgvMain = temp as List<Contenders.Contender>;
                }

                IsOk = true;
                return savedData;
            }
            catch (Exception ex)
            {
                Helpers.ShowGenericPromtForm("התרחשה בעיה בטעינת הנתונים:" + Environment.NewLine + ex.Message.ToString());
                IsOk = false;
                return savedData;
            }
        }

        /// <summary>
        /// deserilize only contenders list from the ContendersFilePath: use when there is no visual contenders and only contntenders (not visual) to load dgvmain for example
        /// </summary>
        /// <param name="IsOk"></param>
        /// <param name="cont"></param>
        /// <returns></returns>
        public virtual List<Contenders.Contender> DeSerialize(out bool IsOk, out List<Contenders.Contender> cont)
        {
            try
            {
                using (Stream stream = File.Open(MartialArts.BinaryFiles.ContendersFilePath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var temp = bformatter.Deserialize(stream);
                    cont = temp as List<Contenders.Contender>;
                }

                IsOk = true;
                return cont;
            }
            catch (Exception ex)
            {
                Helpers.ShowGenericPromtForm("התרחשה בעיה בטעינת הנתונים:" + Environment.NewLine + ex.Message.ToString());
                IsOk = false;
                cont = null;
                return null;
            }
        }


    }



    class SerializeDataSaveAs : SerializeData
    {
        ProjectsSavedAsBinaryFiles paths;
        public SerializeDataSaveAs(Visual.VisualLeagueEvent.UndoStruct Undostruct, ProjectsSavedAsBinaryFiles pathData)
        {
            VisualObjects = Undostruct;
            paths = pathData;
        }


        public override bool Serialize()
        {
            if (VisualObjects._VisualBracketsList == null)
            {
                throw new Exception("you must use the oveloaded constructor and pass him Visual.VisualLeagueEvent.UndoStruct object in order to serialize");
            }

            try
            {
                // Serialize all visual contenders
                using (Stream stream = File.Open(paths.SavedAllContsBinaryFilePath, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, VisualObjects.AllVisualContenders.Select(x => x.Contender).ToList());
                }
                // Serialize visual brackets
                using (Stream stream = File.Open(paths.SavedVbBinaryFilePath, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, VisualObjects._VisualBracketsList.Select(x => x.Bracket).ToList());
                }
                // Serialize unplaced visual contenders
                using (Stream stream = File.Open(paths.SavedUnplacedVcBinaryFilePath, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, VisualObjects._VisualUnplacedBracketsList.Select(x => x.Contender).ToList());
                }

                // Serialize data from datagridview (dgvmain)
                using (Stream stream = File.Open(paths.ContendersFilePath, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, VisualObjects.ContendersInsideDgvMain);
                }

                return true;
            }

            catch (Exception ex)
            {
                Helpers.ShowGenericPromtForm("התרחשה בעיה בשמירת הנתונים:" + Environment.NewLine + ex.Message.ToString());
                return false;
            }

        }


        ///  overide function to save only contenders (not visaul contenders) in case visual brackets did not create yet and there is only datagridview
        /// </summary>
        /// <param name="contenders">list of contenders</param>
        /// <returns></returns>
        public override bool Serialize(List<Contenders.Contender> contenders)
        {

            try
            {
                // Serialize all visual contenders
                using (Stream stream = File.Open(paths.ContendersFilePath, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, contenders);
                }

                // serilize the other files with empty undo struct in order to know that only contenders where saved in the last time
                setSavedFilesToNull();

                return true;
            }



            catch (Exception ex)
            {
                Helpers.ShowGenericPromtForm("התרחשה בעיה בשמירת הנתונים:" + Environment.NewLine + ex.Message.ToString());
                return false;
            }

        }

        // note that this method is only for safty because after brackets are saved user can not cancel them,
        // so this method will run only when brackets has not created yet (but the files will be created 
        // empty for the next time that the user will save also brackets)
        protected override void setSavedFilesToNull()
        {
            // Serialize all visual contenders
            using (Stream stream = File.Open(paths.SavedAllContsBinaryFilePath, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, "");
            }
            // Serialize visual brackets
            using (Stream stream = File.Open(paths.SavedVbBinaryFilePath, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, "");
            }
            // Serialize unplaced visual contenders
            using (Stream stream = File.Open(paths.SavedUnplacedVcBinaryFilePath, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, "");
            }
        }

        public override BracketsBuilder DeSerialize(out bool IsOk)
        {
            BracketsBuilder savedData = new BracketsBuilder();

            try
            {

                // DeSerialize visual brackets
                using (Stream stream = File.Open(paths.SavedVbBinaryFilePath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var temp = bformatter.Deserialize(stream);
                    savedData.BracketsList = temp as List<Bracket>;
                }

                // DeSerialize unplaced visual contenders
                using (Stream stream = File.Open(paths.SavedUnplacedVcBinaryFilePath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var temp = bformatter.Deserialize(stream);
                    savedData.ContendersList = temp as List<Contenders.Contender>;
                }

                // DeSerialize data from datagridview
                using (Stream stream = File.Open(paths.ContendersFilePath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var temp = bformatter.Deserialize(stream);
                    savedData.ContendersFromDgvMain = temp as List<Contenders.Contender>;
                }

                IsOk = true;
                return savedData;
            }
            catch (Exception ex)
            {
                Helpers.ShowGenericPromtForm("התרחשה בעיה בטעינת הנתונים:" + Environment.NewLine + ex.Message.ToString());
                IsOk = false;
                return savedData;
            }
        }

        /// <summary>
        /// deserilize only contenders list from the ContendersFilePath: use when there is no visual contenders and only contntenders (not visual) to load dgvmain for example
        /// </summary>
        /// <param name="IsOk"></param>
        /// <param name="cont"></param>
        /// <returns></returns>
        public override List<Contenders.Contender> DeSerialize(out bool IsOk, out List<Contenders.Contender> cont)
        {
            try
            {
                using (Stream stream = File.Open(paths.ContendersFilePath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var temp = bformatter.Deserialize(stream);
                    cont = temp as List<Contenders.Contender>;
                }

                IsOk = true;
                return cont;
            }
            catch (Exception ex)
            {
                Helpers.ShowGenericPromtForm("התרחשה בעיה בטעינת הנתונים:" + Environment.NewLine + ex.Message.ToString());
                IsOk = false;
                cont = null;
                return null;
            }
        }
    }
}
