
SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


CREATE DATABASE IF NOT EXISTS `weather_for_researchers` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `weather_for_researchers`;


CREATE TABLE `accesstoken` (
  `Id` int(11) NOT NULL,
  `Hash` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Salt` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Time` datetime DEFAULT NULL,
  `UserId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `accesstoken`
--

INSERT INTO `accesstoken` (`Id`, `Hash`, `Salt`, `Time`, `UserId`) VALUES
(6, 'dfLe3QGK9iQcuyq5xMUQyWdJqooFnSK4WakdJRzhPL8=', '151-124-12-193-115-244-244-148-122-86-239-154-34-173-82-73', '2025-08-08 14:57:27', 20);


--
-- Indexes for table `accesstoken`
--
ALTER TABLE `accesstoken`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Hash` (`Hash`),
  ADD UNIQUE KEY `uc_hash` (`Hash`),
  ADD KEY `UserId` (`UserId`),
  ADD KEY `idx_time` (`Time`);

--
-- AUTO_INCREMENT for table `accesstoken`
--
ALTER TABLE `accesstoken`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Constraints for table `accesstoken`
--
ALTER TABLE `accesstoken`
  ADD CONSTRAINT `accesstoken_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`);


DELIMITER $$
--
-- Events
--
CREATE DEFINER=`root`@`localhost` EVENT `auto_delete_old_tokens` ON SCHEDULE EVERY 5 MINUTE STARTS '2025-08-08 18:11:03' ON COMPLETION NOT PRESERVE ENABLE DO BEGIN
    DELETE FROM weather_for_researchers.accesstoken
    WHERE Time < NOW() - INTERVAL 7 DAY
    LIMIT 1000;
END$$

DELIMITER ;
COMMIT;