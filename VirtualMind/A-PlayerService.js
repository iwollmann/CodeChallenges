var PlayerService = {
  getPlayerTeamId: function (playerId) {
    return $.ajax({
      url: '/player/' + playerId + '/team'
    });
  },
  getPlayers: function (teamId) {
    return $.ajax({
      url: '/team/' + teamId + '/player',
    });
  },
};

var PlayerDetailsController = {
  playerId: 8,
  showTeammatesClick: async function () {

    const teamId = await PlayerService.getPlayerTeamId(this.playerId);
    const playerList = await PlayerService.getPlayers(teamId); 
    //Render playerList
  },
};


PlayerDetailsController.showTeammatesClick();