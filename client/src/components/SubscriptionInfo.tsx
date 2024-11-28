import { Subscription } from '../contracts/Game'

interface SubscriptionInfoProps {
	game: Subscription
}

const SubscriptionInfo: React.FC<SubscriptionInfoProps> = ({ game }) => {
	return (
		<div>
			<strong>Subscription Period:</strong> {game.subscriptionPeriodInDays} days
		</div>
	)
}

export default SubscriptionInfo
