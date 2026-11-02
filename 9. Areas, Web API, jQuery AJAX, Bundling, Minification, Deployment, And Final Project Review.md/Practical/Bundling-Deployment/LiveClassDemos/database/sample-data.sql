IF NOT EXISTS (SELECT 1 FROM [Complaints])
INSERT INTO [Complaints]([Title],[Description],[Category],[Priority],[Status],[CreatedDate],[IsArchived]) VALUES
(N'Water leakage',N'Water is leaking near the parking area.',N'Water',N'High',N'New',SYSDATETIME(),0),
(N'Lift maintenance',N'Lift requires routine maintenance.',N'Maintenance',N'Medium',N'InProgress',SYSDATETIME(),0);
