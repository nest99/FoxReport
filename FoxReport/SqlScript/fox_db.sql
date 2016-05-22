-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        5.7.3-m13 - MySQL Community Server (GPL)
-- 服务器操作系统:                      Win32
-- HeidiSQL 版本:                  9.3.0.4984
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 导出 fox 的数据库结构
CREATE DATABASE IF NOT EXISTS `fox` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci */;
USE `fox`;


-- 导出  表 fox.affair_product 结构
CREATE TABLE IF NOT EXISTS `affair_product` (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '三、重点事务：产品事务Id',
  `Classify` varchar(50) COLLATE utf8_unicode_ci DEFAULT '' COMMENT '分类',
  `Priority` varchar(50) COLLATE utf8_unicode_ci DEFAULT '' COMMENT '优先级',
  `Tracker` varchar(50) COLLATE utf8_unicode_ci DEFAULT '' COMMENT '负责人',
  `Workplan` text COLLATE utf8_unicode_ci COMMENT '工作计划',
  `Progress` text COLLATE utf8_unicode_ci COMMENT '进展情况',
  `UserId` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Week` int(11) DEFAULT '0',
  `IsForeign` int(11) DEFAULT '0',
  `OrderNum` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- 数据导出被取消选择。


-- 导出  表 fox.assist_info 结构
CREATE TABLE IF NOT EXISTS `assist_info` (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '五、协助和支持Id',
  `Content` text COLLATE utf8_unicode_ci COMMENT '内容',
  `UserId` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Week` int(11) DEFAULT '0',
  `IsForeign` int(11) DEFAULT '0',
  `OrderNum` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- 数据导出被取消选择。


-- 导出  表 fox.project_info 结构
CREATE TABLE IF NOT EXISTS `project_info` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ProjectName` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Target` text COLLATE utf8_unicode_ci,
  `Progress` text COLLATE utf8_unicode_ci,
  `Teamwork` text COLLATE utf8_unicode_ci,
  `VersionDetail` text COLLATE utf8_unicode_ci,
  `VersionQuality` text COLLATE utf8_unicode_ci,
  `UserId` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Week` int(11) DEFAULT '0',
  `IsForeign` int(11) DEFAULT '0',
  `OrderNum` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- 数据导出被取消选择。


-- 导出  表 fox.report_info 结构
CREATE TABLE IF NOT EXISTS `report_info` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ReportName` varchar(50) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `Week` int(11) DEFAULT '0',
  `UserId` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `IsForeign` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- 数据导出被取消选择。


-- 导出  表 fox.summary_feedback 结构
CREATE TABLE IF NOT EXISTS `summary_feedback` (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户反馈TOP问题Id',
  `Seq` int(11) NOT NULL DEFAULT '0',
  `Platform` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Issue` text COLLATE utf8_unicode_ci,
  `Tracker` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Status` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `TrackInfo` text COLLATE utf8_unicode_ci,
  `UserId` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Week` int(11) DEFAULT '0',
  `IsForeign` int(11) DEFAULT '0',
  `OrderNum` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- 数据导出被取消选择。


-- 导出  表 fox.summary_suggest 结构
CREATE TABLE IF NOT EXISTS `summary_suggest` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Seq` int(11) DEFAULT NULL,
  `Platform` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `SuggestContent` text COLLATE utf8_unicode_ci,
  `UserCount` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Issue` text COLLATE utf8_unicode_ci,
  `TrackInfo` text COLLATE utf8_unicode_ci,
  `UserId` varchar(50) COLLATE utf8_unicode_ci DEFAULT '0',
  `Week` int(11) DEFAULT '0',
  `IsForeign` int(11) DEFAULT '0',
  `OrderNum` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- 数据导出被取消选择。


-- 导出  表 fox.summary_targetstrategy 结构
CREATE TABLE IF NOT EXISTS `summary_targetstrategy` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ProjectName` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Status` text COLLATE utf8_unicode_ci,
  `Target` text COLLATE utf8_unicode_ci,
  `Strategy` text COLLATE utf8_unicode_ci,
  `UserId` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Week` int(11) DEFAULT '0',
  `IsForeign` int(11) DEFAULT '0',
  `OrderNum` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- 数据导出被取消选择。


-- 导出  表 fox.summary_version 结构
CREATE TABLE IF NOT EXISTS `summary_version` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ProjectName` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Request` text COLLATE utf8_unicode_ci,
  `Publish` text COLLATE utf8_unicode_ci,
  `Risk` text COLLATE utf8_unicode_ci,
  `UserId` varchar(50) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `Week` int(11) NOT NULL DEFAULT '0',
  `IsForeign` int(11) NOT NULL DEFAULT '0',
  `OrderNum` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- 数据导出被取消选择。


-- 导出  表 fox.teamwork_info 结构
CREATE TABLE IF NOT EXISTS `teamwork_info` (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '四、团队工作方式优化Id',
  `Content` text COLLATE utf8_unicode_ci COMMENT '内容',
  `UserId` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Week` int(11) DEFAULT '0',
  `IsForeign` int(11) DEFAULT '0',
  `OrderNum` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- 数据导出被取消选择。


-- 导出  表 fox.userinfo 结构
CREATE TABLE IF NOT EXISTS `userinfo` (
  `UserId` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `UserRole` int(11) DEFAULT '0',
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- 数据导出被取消选择。


-- 导出  表 fox.weekstartendday 结构
CREATE TABLE IF NOT EXISTS `weekstartendday` (
  `YearWeek` int(11) NOT NULL,
  `YearNum` int(11) DEFAULT NULL,
  `WeekNum` int(11) DEFAULT NULL,
  `WeekStart` datetime DEFAULT NULL,
  `WeekEnd` datetime DEFAULT NULL,
  PRIMARY KEY (`YearWeek`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- 数据导出被取消选择。
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;