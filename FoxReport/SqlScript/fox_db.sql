
-- 导出 fox 的数据库结构
CREATE DATABASE IF NOT EXISTS `fox`
USE `fox`;


-- 导出  表 fox.detail_biztarget 结构
CREATE TABLE IF NOT EXISTS `detail_biztarget` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ProjectName` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `ProblemAnalyze` varchar(5000) COLLATE utf8_unicode_ci DEFAULT '',
  `RecentTarget` varchar(5000) COLLATE utf8_unicode_ci DEFAULT '',
  `KeyWork` varchar(5000) COLLATE utf8_unicode_ci DEFAULT '',
  `UserId` int(11) DEFAULT '0',
  `Week` int(11) DEFAULT '0',
  `IsForeign` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;


INSERT INTO `detail_biztarget` (`Id`, `ProjectName`, `ProblemAnalyze`, `RecentTarget`, `KeyWork`, `UserId`, `Week`, `IsForeign`) VALUES
	(1, '详细业务项目名称', '问题分析', '近期<div style="color:green">的重</div>点工作', '重点工作内容', 0, 0, 0),
	(2, '项目的名称', '问<div style="color:green">题</div>分<div style="color:green">析</div>内容', '重点工作', '重点内容', 0, 0, 0);


-- 导出  表 fox.summary_targetstrategy 结构
CREATE TABLE IF NOT EXISTS `summary_targetstrategy` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ProjectName` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `Status` varchar(5000) COLLATE utf8_unicode_ci DEFAULT '',
  `Target` varchar(5000) COLLATE utf8_unicode_ci DEFAULT '',
  `Strategy` varchar(5000) COLLATE utf8_unicode_ci DEFAULT '',
  `UserId` int(11) DEFAULT '0',
  `Week` int(11) DEFAULT '0',
  `IsForeign` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

INSERT INTO `summary_targetstrategy` (`Id`, `ProjectName`, `Status`, `Target`, `Strategy`, `UserId`, `Week`, `IsForeign`) VALUES
	(1, 'project name', 'status', 'target', 'strategy', 0, 0, 0),
	(2, 'nnname', '', '目标2', '策略2', 0, 0, 0),
	(3, 'nnname', '', '目标3', '策略3', 0, 0, 0),
	(4, 'nnname', '', '目标4', '策略4', 0, 0, 0),
	(5, 'nnname', '', '目标5', '策略5', 0, 0, 0),
	(6, '66nnname', '', '目标6', '策略6', 0, 0, 0),
	(7, '阿斯蒂芬', '', '山东<div style="color:blue">分公司</div>的', '大公会', 0, 0, 0),
	(8, '阿斯顿发股份和', '', '沙发垫<div style="color:red">沙发上</div>对方沙发上的', 'asdfasdfasdf', 0, 0, 0);

-- 导出  表 fox.userinfo 结构
CREATE TABLE IF NOT EXISTS `userinfo` (
  `UserId` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `UserRole` int(11) DEFAULT '0',
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

