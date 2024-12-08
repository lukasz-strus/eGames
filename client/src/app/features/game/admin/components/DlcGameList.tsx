import React from 'react'
import { Table, Button } from 'react-bootstrap'
import { DlcGame } from '../../../../core/contracts/Game'

interface DlcGameListProps {
	dlcGames: DlcGame[]
	onEdit: (index: number) => void
	onDelete: (index: number) => void
}

const DlcGameList: React.FC<DlcGameListProps> = ({ dlcGames, onEdit, onDelete }) => {
	return (
		<Table bordered>
			<thead>
				<tr>
					<th>Name</th>
					<th>Price</th>
					<th>Actions</th>
				</tr>
			</thead>
			<tbody>
				{dlcGames.map((dlc, index) => (
					<tr key={dlc.id || index}>
						<td>{dlc.name}</td>
						<td>{dlc.amount}</td>
						<td className='text-center'>
							<Button variant='warning' className='me-2' onClick={() => onEdit(index)}>
								Edit
							</Button>
							<Button variant='danger' onClick={() => onDelete(index)}>
								Remove
							</Button>
						</td>
					</tr>
				))}
			</tbody>
		</Table>
	)
}

export default DlcGameList
