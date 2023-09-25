CREATE TABLE [dbo].[Schedules] (
    [ScheduleId]    INT            IDENTITY (1, 1) NOT NULL,
    [StartTime]     DATETIME       NOT NULL,
    [EndTime]       DATETIME       NOT NULL,
    [IsOccupied]    BIT            NOT NULL,
    [ClinicId]      INT            NOT NULL,
    [AppointmentId] INT            NULL,
    CONSTRAINT [PK_dbo.Schedules] PRIMARY KEY CLUSTERED ([ScheduleId] ASC),
    CONSTRAINT [FK_dbo.Schedules_dbo.Clinics_ClinicId] FOREIGN KEY ([ClinicId]) REFERENCES [dbo].[Clinics] ([Id]),
    CONSTRAINT [FK_dbo.Schedules_dbo.Appointments_AppointmentId] FOREIGN KEY ([AppointmentId]) REFERENCES [dbo].[Appointments] ([AppointmentId])
);


INSERT INTO [dbo].[Schedules]
    (StartTime, EndTime, IsOccupied, ClinicId, AppointmentId)
VALUES
    ('09:00:00', '10:00:00', 0, 1, NULL),
    ('10:00:00', '11:00:00', 0, 1, NULL),
    ('11:00:00', '12:00:00', 0, 1, NULL),
    ('12:00:00', '13:00:00', 0, 1, NULL);


