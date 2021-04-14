// This is a react component that is technically functional,
// but would be very hard to maintain because of it's size.

// It's easier to write tests for smaller components that pass
// data between them. Rewrite this component so that it could be
// rendered from somewhere else by using these lines.

// const checkboxes = [0, 1, 2];

// <Form>
// 	checkboxes.map(id =>
// 		<Checkbox key={id} id={id}/>
// 	)
// </Form>

// or (easier but less impresive)

// <Form checkboxes={checkboxes} />

// If you decide to do the second option you MUST STILL create and
// render a Checkbox Component inside the Form Component
function Form({ children }) {
    return(
    	<div className="form">
        {children}
      </div>
    );
};

function Checkbox({ id, value, onChange}) {
    return(
    <div className="checkbox-wrapper">
        <span>checkbox {id}</span>
        <input value={value} onChange={onChange} type="checkbox" />
    </div>);
};

class FormWithChecks extends React.Component { 
    constructor() {
		super();
		this.state = {
			checked: [false, false, false]
		};
	}

  checkboxOnCheck(id) {
		const checked = this.state.checked.map((value, index) => {
			if(id === index) {
				return !value;
			}
			return value;
		});

		this.setState({ checked });
	}

    render() {
		const checked = this.state.checked;
      const checkboxes = [0, 1, 2];
		return (
			<Form>
				<span>Checked boxes: {checked}</span>
                {checkboxes.map(id =>
                    <Checkbox key={id} id={id} onChange={() => this.checkboxOnCheck(id)}/>
                )}
			</Form>
		)
	}
}

ReactDOM.render(
  <FormWithChecks />,
  document.getElementById('container')
);