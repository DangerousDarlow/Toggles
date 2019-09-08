import React, { useState } from "react";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Switch from "@material-ui/core/Switch";
import Box from "@material-ui/core/Box";

export default function Toggles() {
  const [toggle, setToggle] = useState({
    name: "",
    enabled: false
  });

  const handleTextChange = () => event => {
    setToggle({ ...toggle, name: event.target.value });
  };

  const handleSwitchChange = () => event => {
    setToggle({ ...toggle, enabled: event.target.checked });
  };

  const sendToggle = () => {
    if (toggle.name) {
      console.log("Send", toggle);

      fetch("api/Toggles/SetToggle", {
        method: "POST",
        body: JSON.stringify(toggle),
        headers: {
          "Content-Type": "application/json"
        }
      });
    }
  };

  return (
    <div>
      <Box flexDirection="column">
        <Box>
          <TextField
            id="name"
            label="Toggle name"
            value={toggle.name}
            onChange={handleTextChange("name")}
            margin="normal"
          />
        </Box>
        <Box>
          <Switch
            id="enabled"
            checked={toggle.enabled}
            onChange={handleSwitchChange("enabled")}
          />
        </Box>
        <Box>
          <Button variant="contained" color="primary" onClick={sendToggle}>
            SUBMIT
          </Button>
        </Box>
      </Box>
    </div>
  );
}
