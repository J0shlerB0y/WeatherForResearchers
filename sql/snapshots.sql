
SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


CREATE DATABASE IF NOT EXISTS `weather_for_researchers` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `weather_for_researchers`;

--
-- Table structure for table `snapshots`
--

CREATE TABLE `snapshots` (
  `Id` int(11) NOT NULL,
  `CityId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `Time` datetime NOT NULL,
  `weather` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `icon` varchar(200) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `temp` float DEFAULT NULL,
  `temp_feels_like` float DEFAULT NULL,
  `temp_min` float DEFAULT NULL,
  `temp_max` float DEFAULT NULL,
  `pressure` float DEFAULT NULL,
  `humidity` float DEFAULT NULL,
  `wind_speed` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `snapshots`
--

INSERT INTO `snapshots` (`Id`, `CityId`, `UserId`, `Time`, `weather`, `icon`, `temp`, `temp_feels_like`, `temp_min`, `temp_max`, `pressure`, `humidity`, `wind_speed`) VALUES
(43, 1724090496, 20, '2025-08-07 22:32:45', 'clear sky', 'https://openweathermap.org/img/wn/01n@2x.png', 35, 33, 35, 38, 1015, 12, 3),
(44, 1380567274, 20, '2025-08-07 22:33:02', 'clear sky', 'https://openweathermap.org/img/wn/01n@2x.png', 24, 24, 23, 25, 1020, 65, 2),
(45, 1724090496, 20, '2025-08-07 22:37:13', 'clear sky', 'https://openweathermap.org/img/wn/01n@2x.png', 34, 31, 34, 36, 1015, 15, 3);

--
-- Indexes for table `snapshots`
--
ALTER TABLE `snapshots`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `CityId` (`CityId`),
  ADD KEY `UserId` (`UserId`);

--
-- AUTO_INCREMENT for table `snapshots`
--
ALTER TABLE `snapshots`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=47;

--
-- Constraints for table `snapshots`
--
ALTER TABLE `snapshots`
  ADD CONSTRAINT `CityId` FOREIGN KEY (`CityId`) REFERENCES `cities` (`Id`),
  ADD CONSTRAINT `UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`);


COMMIT;