create table Tournament (
	Id int auto_increment primary key,
	Name varchar(50) not null,
	`Date` datetime not null
);

create table Player (
	Id int auto_increment primary key,
	TournamentId int not null,
	Name varchar(50) not null,
	Active bit not null,

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

create table Score (
	Id int auto_increment primary key,
	RoundId int not null,
	PlayerId int not null,
	Score int not null,

	foreign key (RoundId)
		references Round(Id)
		on delete no action,

	foreign key (PlayerId)
		references Player(Id)
		on delete no action
);
