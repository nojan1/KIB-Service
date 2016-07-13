create table Tournament (
	Id int auto_increment primary key,
	Name varchar(50) not null,
	`Date` datetime not null
);

create table Player (
	Id int auto_increment primary key,
	TournamentId int not null,
	Affiliation varchar(50) null,
	Name varchar(50) not null,
	Active tinyint not null,
	CompensationPoints int not null,

	foreign key (TournamentId)
		references Tournament(Id)
		on delete no action
);

create table Round (
	Id int auto_increment primary key,
	RoundNumber int not null,
	TournamentId int not null,

	foreign key (TournamentId)
		references Tournament(Id)
		on delete no action
);

create table Matchup (
	Id int auto_increment primary key,
	RoundId int not null,
	TableNumber int not null,
	Player1Id int not null,
	Player2Id int null,

	foreign key (RoundId)
		references Round(Id)
		on delete no action,

	foreign key (Player1Id)
		references Player(Id)
		on delete no action,
		
	foreign key (Player2Id)
		references Player(Id)
		on delete no action
);

create table Score (
	Id int auto_increment primary key,
	MatchupId int null,
	PlayerId int not null,
	Amount int not null,

	foreign key (PlayerId)
		references Player(Id)
		on delete no action,

	foreign key (MatchupId)
		references Matchup(Id)
		on delete no action
);
