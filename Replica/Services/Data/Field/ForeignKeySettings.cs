using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace X.Config
{
    public class ForeignKeySettings
    {
        [XmlAttribute]
        public string ReferencedStructure { get; set; }
        [XmlAttribute]
        public string KeyOptions { get; set; }
        [XmlAttribute]
        public bool CreateConstraint { get; set; }
    }
    /*    CREATE TABLE `diablo`.`dogtoy` (
  `id` INT NOT NULL,
  `name` VARCHAR(45) NULL,
  `dogID` INT NULL,
  PRIMARY KEY(`id`),
  CONSTRAINT `dogId`
    FOREIGN KEY()
    REFERENCES `diablo`.`dog` ()
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);*/
}