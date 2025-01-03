/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : sql_darkgod

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2024-10-20 22:16:37
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for account
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `acct` varchar(255) NOT NULL,
  `pass` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `level` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `power` int(11) NOT NULL,
  `coin` int(11) NOT NULL,
  `diamond` int(11) NOT NULL,
  `crystal` int(11) NOT NULL,
  `hp` int(11) NOT NULL,
  `ad` int(11) NOT NULL,
  `ap` int(11) NOT NULL,
  `addef` int(11) NOT NULL,
  `apdef` int(11) NOT NULL,
  `dodge` int(11) NOT NULL,
  `pierce` int(11) NOT NULL,
  `critical` int(11) NOT NULL,
  `guideid` int(11) NOT NULL,
  `strong` varchar(255) NOT NULL,
  `offlinetime` bigint(11) NOT NULL,
  `taskreward` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
