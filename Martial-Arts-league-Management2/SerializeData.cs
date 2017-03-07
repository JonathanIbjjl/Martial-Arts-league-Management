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

                return true;
            }

            catch (Exception ex)
            {
                Helpers.ShowGenericPromtForm("התרחשה בעיה בשמירת הנתונים:" + Environment.NewLine + ex.Message.ToString());
                return false;
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

                return true;
            }

            catch (Exception ex)
            {
                Helpers.ShowGenericPromtForm("התרחשה בעיה בשמירת הנתונים:" + Environment.NewLine + ex.Message.ToString());
                return false;
            }

        }

        public override BracketsBuilder DeSerialize(out bool IsOk)
        {
            BracketsBuilder savedData = new BracketsBuilder();

            try
            {

                // Serialize visual brackets
                using (Stream stream = File.Open(paths.SavedVbBinaryFilePath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var temp = bformatter.Deserialize(stream);
                    savedData.BracketsList = temp as List<Bracket>;
                }

                // Serialize unplaced visual contenders
                using (Stream stream = File.Open(paths.SavedUnplacedVcBinaryFilePath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var temp = bformatter.Deserialize(stream);
                    savedData.ContendersList = temp as List<Contenders.Contender>;
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
    }
}
