/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : sql_darkgod

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2024-10-02 22:10:27
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of account
-- ----------------------------
INSERT INTO `account` VALUES ('1', '789', '789', '夏虹', '5', '650', '150', '3000', '500', '6000', '575', '265', '67', '43', '7', '5', '2', '1001', '0#0#0#0#0#0#');
INSERT INTO `account` VALUES ('2', '123', '123', '秦唯', '3', '70', '50', '500', '20', '800', '75', '265', '67', '43', '7', '5', '2', '1003', '0#0#0#0#0#0#');
INSERT INTO `account` VALUES ('3', '126', '126', '魏然', '2', '80', '10', '1000', '80', '1500', '105', '265', '67', '43', '7', '5', '2', '1002', '0#0#0#0#0#0#');
INSERT INTO `account` VALUES ('4', '333', '222', '安伊', '1', '80', '30', '0', '160', '1000', '45', '265', '67', '43', '7', '5', '2', '1001', '0#0#0#0#0#0#');
INSERT INTO `account` VALUES ('5', '629', '789', '东方昭', '1', '0', '100', '0', '500', '2000', '275', '265', '67', '43', '7', '5', '2', '1001', '0#0#0#0#0#0#');
