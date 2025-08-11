
SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


CREATE DATABASE IF NOT EXISTS `weather_for_researchers` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `weather_for_researchers`;

--
-- Table structure for table `userscities`
--

CREATE TABLE `userscities` (
  `Id` int(11) NOT NULL,
  `CityId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `userscities`
--

INSERT INTO `userscities` (`Id`, `CityId`, `UserId`) VALUES
(40, 1724547365, 20),
(44, 1380567274, 20),
(45, 1320366805, 20);

--
-- Indexes for table `userscities`
--
ALTER TABLE `userscities`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `UserscitiesUserId` (`UserId`),
  ADD KEY `UserscitiesCityId` (`CityId`);

--
-- AUTO_INCREMENT for table `userscities`
--
ALTER TABLE `userscities`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=46;
--
-- Constraints for table `userscities`
--
ALTER TABLE `userscities`
  ADD CONSTRAINT `UserscitiesCityId` FOREIGN KEY (`CityId`) REFERENCES `cities` (`Id`),
  ADD CONSTRAINT `UserscitiesUserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`);

COMMIT;