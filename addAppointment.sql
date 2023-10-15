	INSERT INTO [dbo].[Appointments] 
(
    [ScanPart], 
    [Note], 
    [ClinicId], 
    [Title], 
    [UserID], 
    [IsConfirmed], 
    [AppointmentDate], 
    [StartTime], 
    [EndTime], 
    [FirstName], 
    [LastName], 
    [Birthday], 
    [PhoneNumber], 
    [Email], 
    [Gender], 
    [Image_Id]
)
VALUES 
(
    'Brain', 
    'Patient has complained about frequent headaches.', 
    1, 
    'Neurology Checkup', 
    'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 
    0, 
    '2023-10-18T10:00:00.000', 
    '2023-10-18T10:30:00.000', 
    '2023-10-18T11:15:00.000', 
    'John', 
    'Doe', 
    '1990-05-14T00:00:00.000', 
    '123-456-7890', 
    'john.doe@email.com', 
    'Male', 
    NULL
),
(
    'Chest', 
    'Routine checkup. Previous history of asthma.', 
    2, 
    'Pulmonology Review', 
    'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 
    0, 
    '2023-10-19T14:00:00.000', 
    '2023-10-19T14:15:00.000', 
    '2023-10-19T15:00:00.000', 
    'Jane', 
    'Smith', 
    '1985-08-22T00:00:00.000', 
    '098-765-4321', 
    'jane.smith@email.com', 
    'Female', 
    NULL
);


INSERT INTO [dbo].[Appointments] 
(
    [ScanPart], 
    [Note], 
    [ClinicId], 
    [Title], 
    [UserID], 
    [IsConfirmed], 
    [AppointmentDate], 
    [StartTime], 
    [EndTime], 
    [FirstName], 
    [LastName], 
    [Birthday], 
    [PhoneNumber], 
    [Email], 
    [Gender], 
    [Image_Id]
)
VALUES 
('Brain', 'Patient has headaches.', 1, 'Neurology Checkup', 'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 0, '2023-10-18T10:00:00.000', '2023-10-18T10:30:00.000', '2023-10-18T11:15:00.000', 'John', 'Doe', '1990-05-14T00:00:00.000', '123-456-7890', 'john.doe@email.com', 'Male', NULL),
('Chest', 'History of asthma.', 1, 'Pulmonology Review', 'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 0, '2023-10-19T14:00:00.000', '2023-10-19T14:15:00.000', '2023-10-19T15:00:00.000', 'Jane', 'Smith', '1985-08-22T00:00:00.000', '098-765-4321', 'jane.smith@email.com', 'Female', NULL),
('Leg', 'Previous fracture.', 1, 'Orthopedic Visit', 'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 0, '2023-10-20T12:00:00.000', '2023-10-20T12:30:00.000', '2023-10-20T13:00:00.000', 'Alice', 'Johnson', '1987-02-14T00:00:00.000', '321-654-0987', 'alice.johnson@email.com', 'Female', NULL),
('Abdomen', 'Digestive issues.', 1, 'Gastro Checkup', 'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 0, '2023-10-21T09:00:00.000', '2023-10-21T09:45:00.000', '2023-10-21T10:30:00.000', 'Bob', 'Williams', '1989-10-01T00:00:00.000', '567-890-1234', 'bob.williams@email.com', 'Male', NULL),
('Eye', 'Blurred vision.', 1, 'Ophthalmology Review', 'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 0, '2023-10-22T11:00:00.000', '2023-10-22T11:30:00.000', '2023-10-22T12:00:00.000', 'Carol', 'Brown', '1995-04-05T00:00:00.000', '654-321-7890', 'carol.brown@email.com', 'Female', NULL),
('Ear', 'Hearing loss.', 1, 'ENT Checkup', 'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 0, '2023-10-23T13:00:00.000', '2023-10-23T13:45:00.000', '2023-10-23T14:30:00.000', 'David', 'Davis', '1991-06-11T00:00:00.000', '789-012-3456', 'david.davis@email.com', 'Male', NULL),
('Throat', 'Throat pain.', 1, 'ENT Visit', 'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 0, '2023-10-24T08:00:00.000', '2023-10-24T08:45:00.000', '2023-10-24T09:30:00.000', 'Eva', 'Martin', '1992-12-13T00:00:00.000', '901-234-5678', 'eva.martin@email.com', 'Female', NULL),
('Hand', 'Pain in fingers.', 1, 'Orthopedic Checkup', 'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 0, '2023-10-25T15:00:00.000', '2023-10-25T15:45:00.000', '2023-10-25T16:30:00.000', 'Frank', 'Garcia', '1993-07-23T00:00:00.000', '234-567-8901', 'frank.garcia@email.com', 'Male', NULL),
('Foot', 'Sprain.', 1, 'Orthopedic Review', 'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 0, '2023-10-26T10:00:00.000', '2023-10-26T10:45:00.000', '2023-10-26T11:30:00.000', 'Grace', 'Hernandez', '1994-08-31T00:00:00.000', '345-678-9012', 'grace.hernandez@email.com', 'Female', NULL),
('Teeth', 'Toothache.', 1, 'Dental Checkup', 'fe0b8606-e2c1-4f17-a70f-0e89a054e642', 0, '2023-10-27T16:00:00.000', '2023-10-27T16:30:00.000', '2023-10-27T17:15:00.000', 'Harry', 'Lopez', '1996-09-29T00:00:00.000', '456-789-0123', 'harry.lopez@email.com', 'Male', NULL);

